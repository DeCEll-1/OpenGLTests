using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.SceneFolder
{
    public partial class Scene
    { // this place handles post processing effects
        // we will be using a ping pong way of handling it
        // one effect reads from the ping and writes to pong, now ping is free
        // the next one reads from the pong and writes to ping
        // that way we can have as many post processing effects with just 2 FBOs
        public PingPongFBO pingPong { get; private set; } = new();
        public FBO MainFBO = new(); // this is the main FBO we are writing to for our render
        public List<PostProcess> processes = new List<PostProcess>()
        { };
        private PostProcess passthroughPostProcess =
                new PostProcess(
                    new PostProcessingMaterial(
                        Resources.Shaders[ResourcePaths.Materials.PPWriteFBO.Name]
                    )
                ); // the post process we will use if theresnt any processes
        private int i = 0; // counter so we can switch ping and pong
        private void InitPostProcesses()
        {
            MainFBO.Init(name: "Main");

            MainFBO.Bind();
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);
            MainFBO.Unbind();

            pingPong.Init(MainFBO);

        }
        private void HandlePostProcesses()
        {
            if (processes.Count == 0)
            {
                passthroughPostProcess.Apply(FBOToWriteTo: 0, FBOToReadFrom: MainFBO);
                return;
            }
            pingPong.Clear();

            processes[0].Apply(FBOToWriteTo: pingPong.WriteTo.Handle, FBOToReadFrom: MainFBO); // write to Ping from our main FBO
            pingPong.Swap();
            if (processes.Count > 1)
            {
                for (i = 1; // we already did 0th
                    i < processes.Count - 1; // minus 1 because we want to write to buffer 0 on the last step, which is the screen
                    i++)
                {
                    processes[i].Apply(FBOToWriteTo: pingPong.WriteTo.Handle, FBOToReadFrom: pingPong.ReadFrom); // write to Ping from our main FBO
                    pingPong.Swap();

                }
            }
            FBO.SetToDefaultFBO();

            passthroughPostProcess.Apply(FBOToWriteTo: 0, FBOToReadFrom: pingPong.ReadFrom);// write to the screen
            i = 0;

        }


        public class PingPongFBO
        {
            internal PingPongFBO() { }
            public FBO Ping { get; } = new();
            public FBO Pong { get; } = new();
            private bool writeToPing = true;

            public FBO ReadFrom => writeToPing ? Pong : Ping;
            public FBO WriteTo => writeToPing ? Ping : Pong;

            public void Swap() => writeToPing = !writeToPing;
            internal void Clear()
            {
                Ping.Bind();
                GL.Clear(ClearBufferMask.ColorBufferBit);
                Pong.Bind();
                GL.Clear(ClearBufferMask.ColorBufferBit);
                FBO.SetToDefaultFBO();
            }
            internal void Init(FBO main)
            {
                Ping.Init(name: "Ping", depthStencilTexture: main.DepthStencilTexture);
                Ping.Bind();
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);

                Pong.Init(name: "Pong", depthStencilTexture: main.DepthStencilTexture);
                Pong.Bind();
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);
            }
        }

    }
}
