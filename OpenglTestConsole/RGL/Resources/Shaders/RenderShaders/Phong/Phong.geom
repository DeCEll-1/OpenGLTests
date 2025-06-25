#version 330

layout(triangles) in;
layout(triangle_strip) out;
layout(max_vertices = 3) out;

in vec3 FragPos[];
in vec3 Normal[];
flat in vec3 flatNormal[];
in vec2 TexCoord[];

out vec3 FragPos0; // moved frag pos
out vec3 Normal0; // normals from the cpu
flat out vec3 flatNormal0; // normals from the cpu
out vec2 TexCoord0;

out vec3 EdgeDistance0;

void updateVars(int i) {
    FragPos0 = FragPos[i];
    Normal0 = Normal[i];
    flatNormal0 = flatNormal[i];
    TexCoord0 = TexCoord[i];
}

void main()
{
    // i really, dont know why https://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf
    // makes all these calculations, you can just put 1s in the triangles points and it interpolerates anyways?

    // #region obsolete
    // // vec4 p;

    // // p = gl_in[0].gl_Position;
    // // vec2 p0 = vec2(gViewportMatrix * (p / p.w));

    // // p = gl_in[1].gl_Position;
    // // vec2 p1 = vec2(gViewportMatrix * (p / p.w));

    // // p = gl_in[2].gl_Position;
    // // vec2 p2 = vec2(gViewportMatrix * (p / p.w));

    // // float a = length(p1 - p2);
    // // float b = length(p2 - p0);
    // // float c = length(p1 - p0);

    // // float alpha = acos((b * b + c * c - a * a) / (2.0 * b * c));
    // // float beta = acos((a * a + c * c - b * b) / (2.0 * a * c));

    // // float ha = abs(c * sin(beta));
    // // float hb = abs(c * sin(alpha));
    // // float hc = abs(b * sin(alpha));
    // #endregion

    // #region first vertex of the triangle
    gl_Position = gl_in[0].gl_Position;
    gl_ClipDistance = gl_in[0].gl_ClipDistance;

    updateVars(0);

    EdgeDistance0 = vec3(1., 0.0, 0.0);
    EmitVertex();
    // #endregion

    // #region second vertex of the triangle
    gl_Position = gl_in[1].gl_Position;
    gl_ClipDistance = gl_in[1].gl_ClipDistance;

    updateVars(1);

    EdgeDistance0 = vec3(0.0, 1., 0.0);
    EmitVertex();
    // #endregion

    // #region third vertex of the triangle
    gl_Position = gl_in[2].gl_Position;
    gl_ClipDistance = gl_in[2].gl_ClipDistance;

    updateVars(2);

    EdgeDistance0 = vec3(0.0, 0.0, 1.);
    EmitVertex();
    // #endregion

    EndPrimitive();
}
