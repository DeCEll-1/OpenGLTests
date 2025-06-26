using ImGuiNET;
using RGL.API.Rendering.MeshClasses;
using RGL.API.SceneFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.Misc
{
    public class ImguiMisc
    {

        static long indiceCount = 0;
        public static void RenderSceneDebugInfo(Scene Scene)
        {
            RecursiveListType(Scene.Camera);

            if (ImGui.TreeNodeEx("Meshes"))
            {
                ImGui.Text("Triangle Count: " + indiceCount / 3);
                indiceCount = 0;
                foreach (Mesh mesh in Scene.Meshes.SelectMany(s => s).Concat([Scene.Skybox]).Distinct())
                {
                    indiceCount += mesh.Geometry.IndicesLength;
                    ListMesh(mesh);
                }
                ImGui.TreePop();
            }


            if (ImGui.TreeNodeEx("PP Effects"))
            {

                foreach (var process in Scene.PostProcesses)
                {
                    ListMesh(process.ScreenMesh);
                }

                ImGui.TreePop();
            }
        }

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;


        private static void ListMesh(Mesh mesh)
        {
            if (ImGui.TreeNodeEx(mesh.Name))
            {
                ImGui.Text("Primitive Type: " + mesh.type.ToString());
                ImGui.Text("VAO: " + mesh.VertexArrayObjectPointer);

                RecursiveListType(mesh.Transform, prename: "Transform: ");

                RecursiveListType(mesh.Material, prename: "Material: ");

                RecursiveListType(mesh.Geometry, prename: "Geometry: ");


                if (ImGui.TreeNodeEx("Caps To Enable: " + mesh.CapsToEnable.Count))
                {
                    mesh.CapsToEnable.ForEach(s => ImGui.Text(s.ToString()));
                    ImGui.TreePop();
                }
                if (ImGui.TreeNodeEx("Caps To Disable: " + mesh.CapsToDisable.Count))
                {
                    mesh.CapsToDisable.ForEach(s => ImGui.Text(s.ToString()));
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Before Render: " + (mesh.BeforeRender == null ? "NO_FUNC" : mesh.BeforeRender?.Method.Name)))
                {
                    if (mesh.BeforeRender != null)
                        ImGui.Text(ReflectionMisc.GetDelegateeBody(mesh.BeforeRender));
                    ImGui.TreePop();
                }
                if (ImGui.TreeNodeEx("After Render: " + (mesh.AfterRender == null ? "NO_FUNC" : mesh.AfterRender?.Method.Name)))
                {
                    if (mesh.AfterRender != null)
                        ImGui.Text(ReflectionMisc.GetDelegateeBody(mesh.AfterRender));

                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }
        }
        private static Type[] toStringedTypes = [];
        private static void RecursiveListType(object? itemToList, string name = "", string prename = "")
        {
            if (itemToList == null)
                return;
            Type type = itemToList.GetType();
            FieldInfo[] fields = type.GetFields(bindingFlags);
            PropertyInfo[] properties = type.GetProperties(bindingFlags);
            if (name != "")
                name = ": " + name;

            if (ImGui.TreeNodeEx(prename + type.Name + name))
            {
                if (ReflectionMisc.OverridesToString(itemToList))
                { // check if the thing we are trying to list variables of already haves a tostring
                    ImGui.Text(itemToList.ToString()); // just tostring and render that if so
                    ImGui.TreePop();
                    return;
                }

                foreach (var field in fields)
                {
                    var fieldVal = field.GetValue(itemToList);
                    if (fieldVal == null)
                        continue;
                    if (
                        field.FieldType.IsPrimitive ||
                        ReflectionMisc.OverridesToString(fieldVal) ||
                        toStringedTypes.Any(field.FieldType.IsSubclassOf) ||
                        field.FieldType.IsEnum
                    )
                    {

                        if (field.FieldType == typeof(string))
                        {
                            if (((string)fieldVal).Contains("\n") || ((string)fieldVal).Contains("\r"))
                            {
                                if (ImGui.TreeNodeEx(field.Name))
                                {
                                    ImGui.Text(fieldVal?.ToString());
                                    ImGui.TreePop();
                                }
                            }
                        }
                        else
                            ImGui.Text(field.Name + ": " + fieldVal?.ToString());

                    }
                    else
                        RecursiveListType(fieldVal, name: field.Name);
                }
                foreach (var property in properties)
                {
                    var propVal = property.GetValue(itemToList);
                    if (propVal == null)
                        continue;
                    if (
                        property.PropertyType.IsPrimitive ||
                        ReflectionMisc.OverridesToString(propVal) ||
                        toStringedTypes.Any(property.PropertyType.IsSubclassOf) ||
                        property.PropertyType.IsEnum
                    )
                    {

                        if (property.PropertyType == typeof(string))
                        {
                            if (((string)propVal).Contains("\n"))
                            {
                                if (ImGui.TreeNodeEx(property.Name))
                                {
                                    ImGui.Text(propVal?.ToString());
                                    ImGui.TreePop();
                                }
                            }
                        }
                        else
                            ImGui.Text(property.Name + ": " + propVal?.ToString());

                    }
                    else
                        RecursiveListType(propVal, name: property.Name);
                }

                if (type.IsArray)
                {
                    ICollection? items = itemToList as ICollection;
                    if (ImGui.TreeNodeEx("Values: "))
                    {
                        foreach (object? item in items)
                        {
                            Type itemType = item.GetType();
                            if (
                                itemType.IsPrimitive ||
                                ReflectionMisc.OverridesToString(item) ||
                                toStringedTypes.Any(itemType.IsSubclassOf) ||
                                itemType.IsEnum
                            )
                                ImGui.Text(itemType.Name + ": " + item?.ToString());
                            else
                                RecursiveListType(item, name: itemType.Name);
                        }
                        ImGui.TreePop();
                    }
                }

                ImGui.TreePop();
            }
        }



    }
}
