using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace TrapezoidExample
{
    public class Engine : GameWindow
    {
        public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //clear color
            GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Enable(EnableCap.DepthTest);

            //lighting and the light source.
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);

            //directional light parallel to the vector {0,0,0} to {1,1,1}.
            float[] lightPosition = { -1f, -1f, -1f, 0f }; // Directional light (w=0)
            float[] lightAmbient = { 0.2f, 0.2f, 0.2f, 1f };
            float[] lightDiffuse = { 1.0f, 1.0f, 1.0f, 1f };

            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                e.Width / (float)e.Height,
                1.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(new Vector3(20, 20, 20), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            //axes
            GL.Disable(EnableCap.Lighting);
            GL.Begin(PrimitiveType.Lines);
            //X-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(10.0, 0.0, 0.0);
            //Y-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 10.0, 0.0);
            //Z-axis
            GL.Color3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 10.0);
            GL.End();
            GL.Enable(EnableCap.Lighting);

            // splitting into two triangles.
            // z = 0 and have normals pointing along (0,0,1).
            GL.Begin(PrimitiveType.Triangles);

            // Triangle 1: bottom-left, bottom-right, top-right.
            // bottom-left vertex (red)
            GL.Normal3(0f, 0f, 1f); //normal
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);

            // bottom-right vertex (green)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(8f, 0f, 0f);

            // top-right vertex (blue)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(10f, 5f, 0f);

            // Triangle 2: bottom-left, top-right, top-left.
            // bottom-left vertex (red)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);

            // top-right vertex (blue)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(10f, 5f, 0f);

            // top-left vertex (yellow)
            GL.Normal3(0f, 0f, 1f);//normal
            GL.Color3(1f, 1f, 0f);
            GL.Vertex3(2f, 5f, 0f);

            GL.End();

            ////triangle outline
            //GL.Disable(EnableCap.Lighting);
            //GL.LineWidth(2.0f);
            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(0.0, 0.0, 0.0); // black outline
            //GL.Vertex3(0f, 0f, 0f);  // A
            //GL.Vertex3(8f, 0f, 0f);  // B
            //GL.Vertex3(10f, 5f, 0f);  // C
            //GL.End();
            //GL.Enable(EnableCap.Lighting);

            ////triangle outline
            //GL.Disable(EnableCap.Lighting);
            //GL.LineWidth(2.0f);
            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(0.0, 0.0, 0.0); // black outline
            //GL.Vertex3(0f, 0f, 0f);  // A
            //GL.Vertex3(2f, 5f, 0f);  // B
            //GL.Vertex3(10f, 5f, 0f);  // C
            //GL.End();
            //GL.Enable(EnableCap.Lighting);

            ////outline
            //GL.Disable(EnableCap.Lighting);
            //GL.LineWidth(2.0f);
            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(0.0, 0.0, 0.0); // black outline
            //GL.Vertex3(0f, 0f, 0f);  // A
            //GL.Vertex3(8f, 0f, 0f);  // B
            //GL.Vertex3(10f, 5f, 0f);  // C
            //GL.Vertex3(2f, 5f, 0f);  // D
            //GL.End();
            //GL.Enable(EnableCap.Lighting);

            SwapBuffers();
        }
    }
}
