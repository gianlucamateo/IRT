using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IRT.Viewer
{
	public class Camera
	{
		public Matrix CameraMatrix { get { return _camMatrix; } private set { this._camMatrix = value; } }
		public Matrix ViewMatrix { get { return Matrix.Invert(this.CameraMatrix); } }
		public Matrix ProjectionMatrix { get { return _projectionMatrix; } private set { _projectionMatrix = value; } }

		private Matrix _camMatrix, _projectionMatrix;

		public Vector3 Position { get; set; }

		public float Fov
		{
			get { return _fov; }
			set
			{
				_fov = MathHelper.Clamp (value, MathHelper.ToRadians (10f), MathHelper.PiOver2);
				UpdateProperties ();
			}
		}
		public float Aspect { get { return _aspect; } set { _aspect = value; UpdateProperties (); } }

		private float _fov, _aspect;

		public Camera (Vector3 position, float aspect = 16f/9f, float fov = MathHelper.PiOver4)
		{
			this.Position = position;

			this._fov = fov;
			this._aspect = aspect;

			UpdateProperties ();

			this._camMatrix = Matrix.CreateLookAt (position, Vector3.Zero, Vector3.UnitY);
			Matrix.Invert(ref _camMatrix, out _camMatrix);
		}

		private void UpdateProperties ()
		{
			this.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView (_fov, _aspect, 0.1f, 100f);
		}

		public void Pitch (float amount)
		{
			Matrix trans = Matrix.CreateTranslation (-this.Position);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);

			// Transform x axis
			Vector3 xAxis = Vector3.UnitX;
			ToCamSpace (ref xAxis);

			Matrix rot = Matrix.CreateFromAxisAngle (xAxis, amount);
			Matrix.Multiply (ref _camMatrix, ref rot, out _camMatrix);

			Matrix.Invert(ref trans, out trans);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);
		}

		public void Yaw (float amount)
		{
			Matrix trans = Matrix.CreateTranslation (-this.Position);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);

			Matrix rot = Matrix.CreateRotationY (amount);
			Matrix.Multiply(ref _camMatrix, ref rot, out _camMatrix);

			Matrix.Invert(ref trans, out trans);
			Matrix.Multiply(ref _camMatrix, ref trans, out _camMatrix);
		}

		public void Zoom (float amount)
		{
			Fov += amount;
		}

		public void Move (float amount)
		{
			Vector3 unitZ = Vector3.UnitZ;
			ToCamSpace (ref unitZ);
			Matrix trans = Matrix.CreateTranslation (unitZ * amount);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);
			Position = _camMatrix.Translation;
		}

		public void SideStep (float amount)
		{
			Vector3 unitX = Vector3.UnitX;
			ToCamSpace (ref unitX);
			Matrix trans = Matrix.CreateTranslation (unitX * amount);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);
			Position = _camMatrix.Translation;
		}

		public void Elevate (float amount)
		{
			Matrix trans = Matrix.CreateTranslation (Vector3.UnitY * amount);
			Matrix.Multiply (ref _camMatrix, ref trans, out _camMatrix);
			Position = _camMatrix.Translation;
		}

		private void ToCamSpace (ref Vector3 axis)
		{
			Vector3.TransformNormal(ref axis, ref _camMatrix, out axis);
		}

		private int mx, my;
		public void Update (KeyboardState keyboard, MouseState mouse)
		{
			float speedModifier = 1.0f;

			if (keyboard.IsKeyDown(Keys.LeftControl))
			{
				speedModifier *= 0.1f;
			}
			if (keyboard.IsKeyDown(Keys.LeftShift))
			{
				speedModifier *= 5f;
			}
			if (keyboard.IsKeyDown(Keys.W))
			{
				Move (-0.1f * speedModifier);
			}
			if (keyboard.IsKeyDown(Keys.A)) {
				SideStep (-0.1f * speedModifier);
			}
			if (keyboard.IsKeyDown(Keys.S))
			{
				Move (0.1f * speedModifier);
			}
			if (keyboard.IsKeyDown(Keys.D))
			{
				SideStep (0.1f * speedModifier);
			}
			if (keyboard.IsKeyDown(Keys.Q))
			{
				Elevate (0.1f * speedModifier);
			}
			if (keyboard.IsKeyDown(Keys.Y))
			{
				Elevate (-0.1f * speedModifier);
			}

			int x = mouse.X;
			int y = mouse.Y;

			int move = mouse.RightButton == ButtonState.Pressed ? 1 : 0;

			int dx = x - mx;
			int dy = y - my;

			Yaw(-dx / 300f * move);
			Pitch(-dy / 300f * move);

			mx = x;
			my = y;
		}
	}
}

