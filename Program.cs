using System.IO;
using System.Reflection;
using System;
using System.Windows.Forms;
using RimWorld;
using Verse;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Mod_Class_Dumper
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.Write(args[0]);
            if (args.Length == 0)
            {
                args[0] = Console.ReadLine();
            }
            try
            {
                Assembly assembly = Assembly.LoadFrom(args[0]);
                List<Type> types = assembly.ExportedTypes.ToList();
                XmlDocument document = new XmlDocument();
                XmlElement assemblyName = document.CreateElement($"{assembly.FullName.Split(',')[0]}");
                document.AppendChild(assemblyName);
                foreach (Type t in types)
                {
                    if (t.IsPublic)
                    {
                        XmlElement ClassName = document.CreateElement("Class");
                        ClassName.SetAttribute("Name", $"{t.Name}");
                        if (t.BaseType != null)
                            ClassName.SetAttribute("Extends", $"{t.BaseType}");
                        assemblyName.AppendChild(ClassName);
                        foreach (FieldInfo t2 in t.GetFields())
                        {
                            string fieldName = t2.Name;
                            string fieldType = t2.FieldType.Name;
                            XmlElement Field = document.CreateElement(fieldName);
                            if (t2.FieldType.IsGenericType)
                            {
                                string[] names = t2.FieldType.GenericTypeArguments.Select(x => x.Name).ToArray();
                                Field.SetAttribute("Type", $"{fieldType.Remove(fieldType.Length - 2, 2)}({string.Join(",", names)})");
                            }
                            else
                                Field.SetAttribute("Type", fieldType);
                            ClassName.AppendChild(Field);
                            if(t2 == t.GetFields().Last())
                            {
                                XmlComment Propertycomment = document.CreateComment("Properties:");
                                ClassName.InsertAfter(Propertycomment, Field);
                            }
                        }
                        foreach (PropertyInfo t3 in t.GetProperties())
                        {
                            string propertyName = t3.Name;
                            string propertyType = t3.PropertyType.Name;
                            XmlElement Property = document.CreateElement(propertyName);
                            if (t3.PropertyType.IsGenericType)
                            {
                                string[] names = t3.PropertyType.GenericTypeArguments.Select(x => x.Name).ToArray();
                                Property.SetAttribute("Returns", $"{propertyType.Remove(propertyType.Length - 2, 2)}({string.Join(",", names)})");
                            }
                            else
                                Property.SetAttribute("Returns", propertyType); 
                            ClassName.AppendChild(Property);
                        }
                    }
                }
                document.Save($"{assembly.FullName.Split(',')[0]}.xml");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
