using OpenTK.Mathematics;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.Textures;
using RGL.Generated.Paths;

namespace RGL.API.SceneFolder
{
    public partial class Scene
    { // this place handles post processing effects
        // we will be using a ping pong way of handling it
        // one effect reads from the ping and writes to pong, now ping is free
        // the next one reads from the pong and writes to ping
        // that way we can have as many post processing effects with just 2 FBOs
        public PingPongFBO pingPong { get; private set; } = new();
        public FBO MainFBO = new(); // this is the main FBO we are writing to for our render
        public List<PostProcess> PostProcesses = new List<PostProcess>()
        { };
        private PostProcess passthroughPostProcess =
                new PostProcess(
                    new PostProcessingMaterial(
                        Resources.Shaders[RGLResources.Shaders.PPWriteFBO.Name]
                    )
                ); // the post process we will use if theresnt any processes
        private int i = 0; // counter so we can switch ping and pong
        private void InitPostProcesses()
        {
            MainFBO.Init(name: "Main", size: new Vector2i(APISettings.SceneResolution.X, APISettings.SceneResolution.Y));

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
            if (PostProcesses.Count == 0)
            {
                passthroughPostProcess.Apply(FBOToWriteTo: 0, FBOToReadFrom: MainFBO);
                return;
            }
            pingPong.Clear();

            PostProcesses[0].Apply(FBOToWriteTo: pingPong.WriteTo.Handle, FBOToReadFrom: MainFBO); // write to Ping from our main FBO
            pingPong.Swap();
            if (PostProcesses.Count > 1)
            {
                for (i = 1; // we already did 0th
                    i < PostProcesses.Count - 1; // minus 1 because we want to write to buffer 0 on the last step, which is the screen
                    i++)
                {
                    PostProcesses[i].Apply(FBOToWriteTo: pingPong.WriteTo.Handle, FBOToReadFrom: pingPong.ReadFrom); // write to Ping from our main FBO
                    pingPong.Swap();

                }
            }
            FBO.SetToDefaultFBO();

            passthroughPostProcess.Apply(FBOToWriteTo: 0, FBOToReadFrom: pingPong.ReadFrom);// write to the screen
            i = 0;

        }

        public void UpdateFBOs()
        {
            this.MainFBO.Dispose();
            this.MainFBO = new FBO();
            this.pingPong.UpdateFBOs();
            this.InitPostProcesses();
        }

        public class PingPongFBO
        {
            internal PingPongFBO() { }
            public FBO Ping { get; private set; } = new();
            public FBO Pong { get; private set; } = new();
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
                Ping.Init(
                    name: "Ping",
                    depthStencilTexture: main.DepthStencilTexture,
                    size: new Vector2i(APISettings.SceneResolution.X, APISettings.SceneResolution.Y)
                );
                Ping.Bind();
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);

                Pong.Init(
                    name: "Pong",
                    depthStencilTexture: main.DepthStencilTexture,
                    size: new Vector2i(APISettings.SceneResolution.X, APISettings.SceneResolution.Y)
                );
                Pong.Bind();
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);
            }

            internal void UpdateFBOs()
            {
                this.Ping.Dispose();
                this.Ping = new();

                this.Pong.Dispose();
                this.Pong = new();
            }
        }

    }
}
