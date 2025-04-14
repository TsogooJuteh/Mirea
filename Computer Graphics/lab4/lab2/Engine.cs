using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Runtime.InteropServices;

namespace lab2
{
    //vertex structure
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector3 Position;
        public Vector3 Color;
        public Vector2 TexCoord;
        public Vector3 Normal;
    }
    public class Engine : GameWindow
    {
        //parallelogram points
        private readonly Vector3 A = new Vector3(0f, 0f, 0f);
        private readonly Vector3 B = new Vector3(2f, 5f, 0f);
        private readonly Vector3 C = new Vector3(10f, 5f, 0f);
        private readonly Vector3 D = new Vector3(8f, 0f, 0f);

        //pyramid point
        private readonly Vector3 M = new Vector3(0f, 0f, 5f);

        //texture
        int texture;
        float angle = 270.0f;

        // vbo/vao/ibo for the parallelogram
        int baseVao, baseVbo, baseIbo;
        int pyramidVao, pyramidVbo, pyramidIbo;

        private Vector3 someMovingPoint;
        private float time = 0f;

        //camera control
        private float cameraAngle = 0f;
        private float cameraDistance = 40f;
        private float cameraHeight = 20f;


        public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.Enable(EnableCap.Texture2D);
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

            //texture
            texture = forimages.LoadTexture("image.png");

            //lighting and the light source.
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1f);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);

            //directional light parallel to the vector {0,0,0} to {1,1,1}.
            //the second exercise
            GL.Enable(EnableCap.Light0);
            float[] lightPosition = { 1f, 1f, 1f, 0f }; // Directional light (-1,0,0)
            float[] lightAmbient = { 0.2f, 0.2f, 0.2f, 1f };
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);

            //light1: the spotlight
            //the third exercise
            GL.Enable(EnableCap.Light1);
            float[] spotPosition = { 0f, 5f, 5f, 1f };
            GL.Light(LightName.Light1, LightParameter.Position, spotPosition);

            float[] spotDiffuse = { 500f, 500f, 500f, 1f };
            GL.Light(LightName.Light1, LightParameter.Diffuse, spotDiffuse);

            float[] spotSpecular = { 100f, 100f, 100f, 1f };
            GL.Light(LightName.Light1, LightParameter.Specular, spotSpecular);

            //calculating direction of spotlight
            float invSqrt2 = 1.0f / MathF.Sqrt(2.0f);
            float[] spotdirection = { 0f, -invSqrt2, -invSqrt2 };
            GL.Light(LightName.Light1, LightParameter.SpotDirection, spotdirection);

            //global ambient
            float[] globalAmbient = { 0f, 0f, 0f, 1f };
            GL.LightModel(LightModelParameter.LightModelAmbient, globalAmbient);

            //set the clipping angle (30) and the spotlight exponent
            GL.Light(LightName.Light1, LightParameter.SpotCutoff, 30f);
            GL.Light(LightName.Light1, LightParameter.SpotExponent, 2f);


            //setting attunation(затухание)
            GL.Light(LightName.Light1, LightParameter.ConstantAttenuation, 0f);
            GL.Light(LightName.Light1, LightParameter.LinearAttenuation, 0f);
            GL.Light(LightName.Light1, LightParameter.QuadraticAttenuation, 1f);

            //setup base parallelogram using vbo,ibo,vao
            SetupBaseParallelogram();

            SetupPyramid();
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

            //check arrow keys to modify camera
            var keyboard = KeyboardState;
            if (keyboard.IsKeyDown(Keys.Left))
                cameraAngle -= 0.1f; //rotate left
            if (keyboard.IsKeyDown(Keys.Right))
                cameraAngle += 0.1f; //rotate right
            if (keyboard.IsKeyDown(Keys.Up))
                cameraDistance = MathF.Max(5f, cameraDistance - 0.01f); //move camera closer
            if (keyboard.IsKeyDown(Keys.Down))
                cameraDistance += 0.01f; //move camera further away

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        private void SetupBaseParallelogram()
        {
            //4 unique vertices
            Vertex[] vertices = new Vertex[]
            {
                new Vertex { Position = A, Color = new Vector3(1f, 0f, 0f), TexCoord = new Vector2(0f, 0f), Normal = new Vector3(0f, 0f, 1f) },
                new Vertex { Position = B, Color = new Vector3(1f, 1f, 0f), TexCoord = new Vector2(0f, 1f), Normal = new Vector3(0f, 0f, 1f) },
                new Vertex { Position = C, Color = new Vector3(0f, 1f, 0f), TexCoord = new Vector2(1f, 1f), Normal = new Vector3(0f, 0f, 1f) },
                new Vertex { Position = D, Color = new Vector3(0f, 0f, 1f), TexCoord = new Vector2(1f, 0f), Normal = new Vector3(0f, 0f, 1f) }
            };

            //2 triangles to for parallelogram
            ushort[] indices = new ushort[]
            {
                0, 2, 3,
                0, 1, 2
            };

            //vertex buffer object vbo
            baseVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, baseVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Marshal.SizeOf<Vertex>(), vertices, BufferUsageHint.StaticDraw);

            //index buffer object ibo
            baseIbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, baseIbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(ushort), indices, BufferUsageHint.StaticDraw);

        }

        private void SetupPyramid()
        {
            //normals
            Vector3 normalABM = ComputeNormal(A, B, M);
            Vector3 normalBCM = ComputeNormal(B, C, M);
            Vector3 normalCDM = ComputeNormal(C, D, M);
            Vector3 normalDAM = ComputeNormal(D, A, M);

            //5 unique vertices:ABCDM
            Vertex VertexA = new Vertex
            {
                Position = A,
                Color = new Vector3(1f, 0f, 0f),
                TexCoord = new Vector2(0f, 0f),
                Normal = Vector3.Normalize(normalABM + normalDAM)
            };
            Vertex VertexB = new Vertex
            {
                Position = B,
                Color = new Vector3(1f, 1f, 0f),
                TexCoord = new Vector2(1f, 0f),
                Normal = Vector3.Normalize(normalABM + normalBCM)
            };
            Vertex VertexC = new Vertex
            {
                Position = C,
                Color = new Vector3(0f, 0f, 1f),
                TexCoord = new Vector2(1f, 1f),
                Normal = Vector3.Normalize(normalBCM + normalCDM)
            };
            Vertex VertexD = new Vertex
            {
                Position = D,
                Color = new Vector3(0f, 1f, 0f),
                TexCoord = new Vector2(0f, 1f),
                Normal = Vector3.Normalize(normalCDM + normalDAM)
            };
            Vertex VertexM = new Vertex
            {
                Position = M,
                Color = new Vector3(0f, 0.5f, 1f),
                TexCoord = new Vector2(0.5f, 0.5f),
                Normal = Vector3.Normalize(normalABM + normalBCM + normalCDM + normalDAM)
            };
            Vertex[] pyramidVertices = new Vertex[] { VertexA, VertexB, VertexC, VertexD, VertexM };

            //defining indices for the pyramid faces
            ushort[] pyramidIndices = new ushort[]
            {
                0, 1, 4,  //face ABM
                1, 2, 4,  //face BCM
                2, 3, 4,  //face CDM
                3, 0, 4   //face DAM
            };

            //create and bind the vbo
            pyramidVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, pyramidVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, pyramidVertices.Length * Marshal.SizeOf<Vertex>(), pyramidVertices, BufferUsageHint.StaticDraw);

            //create and bind the ibo
            pyramidIbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pyramidIbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, pyramidIndices.Length * sizeof(ushort), pyramidIndices, BufferUsageHint.StaticDraw);

        }

        private void DrawPyramid()
        {
            int stride = Marshal.SizeOf<Vertex>();
            //draw the base parallelogram
            GL.BindBuffer(BufferTarget.ArrayBuffer, baseVbo);
            GL.VertexPointer(3, VertexPointerType.Float, stride, IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, stride, (IntPtr)(3 * sizeof(float)));
            GL.TexCoordPointer(2, TexCoordPointerType.Float, stride, (IntPtr)(6 * sizeof(float)));
            GL.NormalPointer(NormalPointerType.Float, stride, (IntPtr)(8 * sizeof(float)));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, baseIbo);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedShort, IntPtr.Zero);

            //draw the pyramid
            GL.BindBuffer(BufferTarget.ArrayBuffer, pyramidVbo);
            GL.VertexPointer(3, VertexPointerType.Float, stride, IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, stride, (IntPtr)(3 * sizeof(float)));
            GL.TexCoordPointer(2, TexCoordPointerType.Float, stride, (IntPtr)(6 * sizeof(float)));
            GL.NormalPointer(NormalPointerType.Float, stride, (IntPtr)(8 * sizeof(float)));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pyramidIbo);
            GL.DrawElements(PrimitiveType.Triangles, 12, DrawElementsType.UnsignedShort, IntPtr.Zero);
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
                100.0f
            );
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

            //calculate cam position
            float radAngle = MathHelper.DegreesToRadians(cameraAngle);
            Vector3 cameraPosition = new Vector3(
                cameraDistance * MathF.Cos(radAngle),
                cameraHeight,
                cameraDistance * MathF.Sin(radAngle)
                );

            //camera
            Matrix4 modelview = Matrix4.LookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            //the light direction (from someMovingPoint -> origin)
            Vector3 dir = Vector3.Normalize(Vector3.Zero - someMovingPoint);
            float[] lightPosition = { dir.X, dir.Y, dir.Z, 0f }; // directional light w=0

            //update light’s position
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);

            //spotlight
            float[] updatespotposition = { 0f, 10f, 10f, 1f };
            GL.Light(LightName.Light1, LightParameter.Position, updatespotposition);
            float invSqrt2 = 1.0f / MathF.Sqrt(2.0f);
            float[] updatedSpotDirection = { 0f, -invSqrt2, -invSqrt2 };
            GL.Light(LightName.Light1, LightParameter.SpotDirection, updatedSpotDirection);

            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Light1);


            //re-enable lighting for the rest of the scene
            GL.Enable(EnableCap.Lighting);
            GL.Color3(1f, 1f, 1f);

            //testing a line
            GL.LineWidth(1f);
            GL.Begin(PrimitiveType.Lines);
            //X-axis
            GL.Color3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(10f, 0f, 0f);
            //Y-axis
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 10f, 0f);
            //Z-axis
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 10f);
            GL.End();

            GL.Enable(EnableCap.Lighting);

            //texture rotation
            GL.MatrixMode(MatrixMode.Texture);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0.5f, 0.5f, 0f);
            GL.Rotate(angle, 0f, 0f, 1f);
            GL.Translate(-0.5, -0.5, 0f);
            GL.MatrixMode(MatrixMode.Modelview);

            //bind the texture.
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            //use fixed function client state to set vertex, color, texture, and normal
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            //drawin overlapping 2 pyramids
            //1st
            DrawPyramid();

            //2nd
            GL.PushMatrix();
            GL.Translate(2f, 6f, 1.5f);
            GL.Rotate(90, 1f, 0f, 0f);
            DrawPyramid();
            GL.PopMatrix();

            //disable client states.
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.MatrixMode(MatrixMode.Texture);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);

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
