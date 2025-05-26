﻿using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public partial class PhongMaterial
    {
        #region pre made materials
        public static PhongMaterial Emerald =>
            new PhongMaterial(
                new Vector3(0.0215f, 0.1745f, 0.0215f),
                new Vector3(0.07568f, 0.61424f, 0.07568f),
                new Vector3(0.633f, 0.727811f, 0.633f),
                0.6f
            );
        public static PhongMaterial Jade =>
            new PhongMaterial(
                new Vector3(0.135f, 0.2225f, 0.1575f),
                new Vector3(0.54f, 0.89f, 0.63f),
                new Vector3(0.316228f, 0.316228f, 0.316228f),
                0.1f
            );
        public static PhongMaterial Obsidian =>
            new PhongMaterial(
                new Vector3(0.05375f, 0.05f, 0.06625f),
                new Vector3(0.18275f, 0.17f, 0.22525f),
                new Vector3(0.332741f, 0.328634f, 0.346435f),
                0.3f
            );
        public static PhongMaterial Pearl =>
            new PhongMaterial(
                new Vector3(0.25f, 0.20725f, 0.20725f),
                new Vector3(1f, 0.829f, 0.829f),
                new Vector3(0.296648f, 0.296648f, 0.296648f),
                0.088f
            );
        public static PhongMaterial Ruby =>
            new PhongMaterial(
                new Vector3(0.1745f, 0.01175f, 0.01175f),
                new Vector3(0.61424f, 0.04136f, 0.04136f),
                new Vector3(0.727811f, 0.626959f, 0.626959f),
                0.6f
            );
        public static PhongMaterial Turquoise =>
            new PhongMaterial(
                new Vector3(0.1f, 0.18725f, 0.1745f),
                new Vector3(0.396f, 0.74151f, 0.69102f),
                new Vector3(0.297254f, 0.30829f, 0.306678f),
                0.1f
            );
        public static PhongMaterial Brass =>
            new PhongMaterial(
                new Vector3(0.329412f, 0.223529f, 0.027451f),
                new Vector3(0.780392f, 0.568627f, 0.113725f),
                new Vector3(0.992157f, 0.941176f, 0.807843f),
                0.21794872f
            );
        public static PhongMaterial Bronze =>
            new PhongMaterial(
                new Vector3(0.2125f, 0.1275f, 0.054f),
                new Vector3(0.714f, 0.4284f, 0.18144f),
                new Vector3(0.393548f, 0.271906f, 0.166721f),
                0.2f
            );
        public static PhongMaterial Chrome =>
            new PhongMaterial(
                new Vector3(0.25f, 0.25f, 0.25f),
                new Vector3(0.4f, 0.4f, 0.4f),
                new Vector3(0.774597f, 0.774597f, 0.774597f),
                0.6f
            );
        public static PhongMaterial Copper =>
            new PhongMaterial(
                new Vector3(0.19125f, 0.0735f, 0.0225f),
                new Vector3(0.7038f, 0.27048f, 0.0828f),
                new Vector3(0.256777f, 0.137622f, 0.086014f),
                0.1f
            );
        public static PhongMaterial Gold =>
            new PhongMaterial(
                new Vector3(0.24725f, 0.1995f, 0.0745f),
                new Vector3(0.75164f, 0.60648f, 0.22648f),
                new Vector3(0.628281f, 0.555802f, 0.366065f),
                0.4f
            );
        public static PhongMaterial Silver =>
            new PhongMaterial(
                new Vector3(0.19225f, 0.19225f, 0.19225f),
                new Vector3(0.50754f, 0.50754f, 0.50754f),
                new Vector3(0.508273f, 0.508273f, 0.508273f),
                0.4f
            );
        public static PhongMaterial BlackPlastic =>
            new PhongMaterial(
                new Vector3(0f, 0f, 0f),
                new Vector3(0.01f, 0.01f, 0.01f),
                new Vector3(0.5f, 0.5f, 0.5f),
                0.25f
            );
        public static PhongMaterial CyanPlastic =>
            new PhongMaterial(
                new Vector3(0f, 0.1f, 0.06f),
                new Vector3(0f, 0.50980392f, 0.50980392f),
                new Vector3(0.50196078f, 0.50196078f, 0.50196078f),
                0.25f
            );
        public static PhongMaterial GreenPlastic =>
            new PhongMaterial(
                new Vector3(0f, 0f, 0f),
                new Vector3(0.1f, 0.35f, 0.1f),
                new Vector3(0.45f, 0.55f, 0.45f),
                0.25f
            );
        public static PhongMaterial RedPlastic =>
            new PhongMaterial(
                new Vector3(0f, 0f, 0f),
                new Vector3(0.5f, 0f, 0f),
                new Vector3(0.7f, 0.6f, 0.6f),
                0.25f
            );
        public static PhongMaterial WhitePlastic =>
            new PhongMaterial(
                new Vector3(0f, 0f, 0f),
                new Vector3(0.55f, 0.55f, 0.55f),
                new Vector3(0.7f, 0.7f, 0.7f),
                0.25f
            );
        public static PhongMaterial YellowPlastic =>
            new PhongMaterial(
                new Vector3(0f, 0f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
                new Vector3(0.6f, 0.6f, 0.5f),
                0.25f
            );
        public static PhongMaterial BlackRubber =>
            new PhongMaterial(
                new Vector3(0.02f, 0.02f, 0.02f),
                new Vector3(0.01f, 0.01f, 0.01f),
                new Vector3(0.4f, 0.4f, 0.4f),
                0.078125f
            );
        public static PhongMaterial CyanRubber =>
            new PhongMaterial(
                new Vector3(0f, 0.05f, 0.05f),
                new Vector3(0.4f, 0.5f, 0.5f),
                new Vector3(0.04f, 0.7f, 0.7f),
                0.078125f
            );
        public static PhongMaterial GreenRubber =>
            new PhongMaterial(
                new Vector3(0f, 0.05f, 0f),
                new Vector3(0.4f, 0.5f, 0.4f),
                new Vector3(0.04f, 0.7f, 0.04f),
                0.078125f
            );
        public static PhongMaterial RedRubber =>
            new PhongMaterial(
                new Vector3(0.05f, 0f, 0f),
                new Vector3(0.5f, 0.4f, 0.4f),
                new Vector3(0.7f, 0.04f, 0.04f),
                0.078125f
            );
        public static PhongMaterial WhiteRubber =>
            new PhongMaterial(
                new Vector3(0.05f, 0.05f, 0.05f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.7f, 0.7f, 0.7f),
                0.078125f
            );
        public static PhongMaterial YellowRubber =>
            new PhongMaterial(
                new Vector3(0.05f, 0.05f, 0f),
                new Vector3(0.5f, 0.5f, 0.4f),
                new Vector3(0.7f, 0.7f, 0.04f),
                0.078125f
            );

        #endregion
    }
}
