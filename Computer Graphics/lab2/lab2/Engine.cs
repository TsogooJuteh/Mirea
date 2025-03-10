using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace lab2
{
    public class Engine : GameWindow
    {
        //parallelogram points
        private readonly Vector3 A = new Vector3(0f, 0f, 0f);
        private readonly Vector3 B = new Vector3(2f, 5f, 0f);
        private readonly Vector3 C = new Vector3(10f, 5f, 0f);
        private readonly Vector3 D = new Vector3(8f, 0f, 0f);
        
        //pyramid point
        private readonly Vector3 M = new Vector3(0f, 0f, 5f);

        private Vector3 someMovingPoint;
        private float time = 0f;
        public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Version));

            //clear color
            GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Enable(EnableCap.DepthTest);

            //lighting and the light source.
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Enable(EnableCap.ColorMaterial);


            //directional light parallel to the vector {0,0,0} to {1,1,1}.
            float[] lightPosition = { 1f, 1f, 1f, 0f }; // Directional light (-1,0,0)
            float[] lightAmbient = { 0.2f, 0.2f, 0.2f, 1f };

            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            //time by the elapsed frame time.
            time += (float)args.Time;

            //circle the XZ-plane, Y=10
            float radius = 15f;
            float x = radius * MathF.Cos(time);
            float z = radius * MathF.Sin(time);
            float y = 10f;

            //storing the position
            someMovingPoint = new Vector3(x, y, z);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                (float)e.Width / e.Height,
                1.0f,
                100.0f);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private Vector3 ComputeNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            Vector3 edge0 = v1 - v0;
            Vector3 edge1 = v2 - v0;
            return Vector3.Normalize(Vector3.Cross(edge0, edge1));
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //camera
            Matrix4 modelview = Matrix4.LookAt(new Vector3(20, 20, 20), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            //the light direction (from someMovingPoint -> origin)
            Vector3 dir = Vector3.Normalize(Vector3.Zero - someMovingPoint);
            float[] lightPosition = { dir.X, dir.Y, dir.Z, 0f }; // directional light w=0

            //update light’s position
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);

            GL.Disable(EnableCap.Lighting);

            //testing a line
            GL.Begin(PrimitiveType.Lines);
            //X-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(-10.0, 0.0, 0.0);
            GL.Vertex3(10.0, 0.0, 0.0);
            //Y-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, -10.0, 0.0);
            GL.Vertex3(0.0, 10.0, 0.0);
            //Z-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, -100.0);
            GL.Vertex3(0.0, 0.0, 10.0);

            GL.End();
            GL.Enable(EnableCap.Lighting);

            // splitting into two triangles.
            // z = 0 and have normals pointing along (0,0,1).
            GL.Begin(PrimitiveType.Triangles);

            // Triangle 1: ACD
            // bottom-left vertex (red)
            GL.Normal3(0f, 0f, 1f); //normal
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(A);

            // bottom-right vertex (green)
            GL.Normal3(0f, 0f, 1f);//normal     
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(C);

            // top-right vertex (blue)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(D);

            // Triangle 2: ABC
            // bottom-left vertex (red)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(A);

            // top-right vertex (blue)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(C);

            // top-left vertex (yellow)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(1f, 1f, 0f);
            GL.Vertex3(B);

            GL.End();

            //building pyramid
            GL.Begin(PrimitiveType.Triangles);

            //ABM
            Vector3 normal1 = ComputeNormal(A, B, M);
            GL.Normal3(normal1);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(A);
            GL.Color3(1f, 1f, 0f);
            GL.Vertex3(B);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(M);

            //BCM
            Vector3 normal2 = ComputeNormal(B, C, M);
            GL.Normal3(normal2);
            GL.Color3(1f, 1f, 0f);
            GL.Vertex3(B);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(C);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(M);


            //CDM
            Vector3 normal3 = ComputeNormal(C, D, M);
            GL.Normal3(normal3);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(C);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(D);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(M);


            //DAM
            Vector3 normal4 = ComputeNormal(D, A, M);
            GL.Normal3(normal4);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(D);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(A);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(M);

            GL.End();

            GL.Enable(EnableCap.Lighting);

            //x - y + z + 10 = 0
            //the center of ellipse (0,0,-10)
            Vector3 center = new Vector3(0f, 0f, -10f);
            float a = 5f;
            float b = 3f;

            //plane normal n = (1, -1, 1)
            Vector3 n = Vector3.Normalize(new Vector3(1f, -1f, 1f));
            //tangent vector
            Vector3 arbitrary = new Vector3(1f, 0f, 0f);
            Vector3 u = Vector3.Normalize(arbitrary - Vector3.Dot(arbitrary, n) * n);
            //second tangent from w orthogonal to u and n
            Vector3 w = Vector3.Normalize(Vector3.Cross(n, u));

            //ellipse
            const int segments = 36;
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0f, 0.8f, 0f); //emerald
            for (int i = 0; i < segments; i++)
            {
                float theta = i * MathHelper.TwoPi / segments;
                Vector3 point = center + a * (float)Math.Cos(theta) * u + b * (float)Math.Sin(theta) * w;
                GL.Vertex3(point);
            }
            GL.End();
            GL.LineWidth(1f);

            SwapBuffers();
        }
    }
}
