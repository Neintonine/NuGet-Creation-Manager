using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using DrWPF.Windows.Data;

namespace NUGETManager.Storage
{
    [Serializable]
    public class Project
    {
        public int FilledVersionIndex
        {
            get
            {
                int versionIndex = 0;

                foreach (Version v in Versions)
                {
                    if (!string.IsNullOrEmpty(v.Name))
                    {
                        versionIndex = Versions.IndexOf(v);
                        break;
                    }
                }

                return versionIndex;
            }
        }

        public bool DeletePackageAfterUpload;

        public bool CompileFile;
        public string CompileFilePath;

        public string ProjectFile;

        public string ProjectPath;

        public string Title = "Default Title";
        public string APIKey = "Replace your key with this!";
        public string Description;

        public string ProjectURL;
        public string License = "MIT";

        public string Authors;
        public string Tags;

        public string Identifier;

        public string Copyright;

        public NUGETFile Icon;

        public bool RequireLicenseAcceptance;

        public string TargetLocation;

        public ObservableDictionary<NUGETFramework, Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>> References = new ObservableDictionary<NUGETFramework, Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>>();
        public ObservableCollection<NUGETFile> Files = new ObservableCollection<NUGETFile>();

        public ObservableCollection<Storage.Version> Versions = new ObservableCollection<Storage.Version>();

        public Project()
        {
            References.Add(NUGETFramework.Any, new Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>(new ObservableCollection<NUGETFile>(), new ObservableCollection<NUGETDependency>()));
            Versions.Add(new Version() {Name="1.0.0", ReleaseNotes = "First Release"});
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Identifier) ? Identifier : Title;
        }

        public void Save()
        {
            string filename = (!string.IsNullOrEmpty(Identifier) ? Identifier : Title);
            SaveManager.Save("Projects\\"+filename , this);
            ProjectFile = SaveManager.AppDataPath + "\\Projects\\" + filename + ".bin";
        }

        public XmlDocument CreateNuspec()
        {
            int versionIndex = FilledVersionIndex;



            Dictionary<string, Tuple<string, bool>> metadataDictionary = new Dictionary<string, Tuple<string, bool>>()
            {
                {"id", new Tuple<string, bool>(Identifier, true)},
                {"version", new Tuple<string, bool>(Versions[versionIndex].Name, true)},
                {"title", new Tuple<string, bool>(Title, false)},
                {"authors", new Tuple<string, bool>(Authors, true)},
                {"requireLicenseAcceptance", new Tuple<string, bool>(RequireLicenseAcceptance.ToString().ToLower(), false)},
                {"license", new Tuple<string, bool>(License, false)},
                {"projectUrl", new Tuple<string, bool>(ProjectURL, false)},
                {"description", new Tuple<string, bool>(Description, true)},
                {"releaseNotes", new Tuple<string, bool>(Versions[versionIndex].ReleaseNotes, false)},
                {"copyright", new Tuple<string, bool>(Copyright, false)},
                {"tags", new Tuple<string, bool>(Tags, false)},
                {"icon", new Tuple<string, bool>(Icon != null ? "images/"+Path.GetFileName(Icon.Path) : "", false)}
            };

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(doc.CreateComment(" This was created by the 'NUGET manager' by iedSoftworks. "));

            XmlNode package = doc.CreateElement("package");

            XmlNode metadata = doc.CreateElement("metadata");

            foreach (KeyValuePair<string, Tuple<string, bool>> pair in metadataDictionary)
            {
                if (!pair.Value.Item2 && string.IsNullOrEmpty(pair.Value.Item1)) continue;

                XmlNode title = doc.CreateElement(pair.Key);

                if (pair.Key == "license")
                {
                    XmlAttribute attribute = doc.CreateAttribute("type");
                    attribute.Value = "expression";
                    title.Attributes.Append(attribute);
                }

                title.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty(pair.Value.Item1) ? "Missing" : pair.Value.Item1));
                metadata.AppendChild(title);
            }

            XmlNode dependencies = doc.CreateElement("dependencies");
            XmlNode references = doc.CreateElement("references");
            foreach (KeyValuePair< NUGETFramework, Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>> reference in References)
            {
                NUGETFramework framework = reference.Key;

                ObservableCollection<NUGETFile> fileRefs = reference.Value.Item1;
                ObservableCollection<NUGETDependency> dependencyRefs = reference.Value.Item2;

                if (fileRefs.Count > 0)
                {
                    XmlNode refGroup = doc.CreateElement("group");
                    if (!framework._isAny)
                    {
                        XmlAttribute attribute = doc.CreateAttribute("targetFramework");
                        attribute.Value = framework.ReferenceIdentifier;
                        refGroup.Attributes.Append(attribute);
                    }

                    foreach (NUGETFile file in fileRefs)
                    {
                        XmlNode refer = doc.CreateElement("reference");
                        XmlAttribute attribute = doc.CreateAttribute("file");
                        attribute.Value = Path.GetFileName(file.Path);
                        refer.Attributes.Append(attribute);
                        refGroup.AppendChild(refer);
                    }

                    references.AppendChild(refGroup);
                }

                if (dependencyRefs.Count > 0)
                {
                    XmlNode dependGroup = doc.CreateElement("group");
                    if (!framework._isAny)
                    {
                        XmlAttribute attribute = doc.CreateAttribute("targetFramework");
                        attribute.Value = framework.DependencyIdentifier;
                        dependGroup.Attributes.Append(attribute);
                    }

                    foreach (NUGETDependency dependency in dependencyRefs)
                    {
                        XmlNode refer = doc.CreateElement("dependency");
                        
                        XmlAttribute attribute = doc.CreateAttribute("id");
                        attribute.Value = dependency.Package;

                        XmlAttribute attribute2 = doc.CreateAttribute("version");
                        attribute2.Value = dependency.Version;

                        refer.Attributes.Append(attribute);
                        refer.Attributes.Append(attribute2);
                        dependGroup.AppendChild(refer);
                    }

                    dependencies.AppendChild(dependGroup);
                }
            }

            metadata.AppendChild(dependencies);
            metadata.AppendChild(references);

            package.AppendChild(metadata);

            XmlNode files = doc.CreateElement("files");

            foreach (NUGETFile file in Files)
            {
                XmlNode fileNode = doc.CreateElement("file");

                XmlAttribute src = doc.CreateAttribute("src");
                src.Value = file.Path;

                XmlAttribute target = doc.CreateAttribute("target");
                target.Value = file.StoreLocation;

                fileNode.Attributes.Append(src);
                fileNode.Attributes.Append(target);

                files.AppendChild(fileNode);
            }

            package.AppendChild(files);

            doc.AppendChild(package);

            return doc;
        }
    }
}