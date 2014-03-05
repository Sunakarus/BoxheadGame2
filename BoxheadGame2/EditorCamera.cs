using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxheadGame2
{
    class EditorCamera
    {
        public EditorCamera()
            {
            }
            public Vector2 position = Vector2.Zero;


            public Matrix TransformMatrix()
            {
                return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
            }

    }
}
