using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SephiriaModList.UI
{
    public static class GameObjectExtensions
    {
        public static CanvasRenderer AddCanvasRenderer(this GameObject gameObject)
        {
            var renderer = gameObject.AddComponent<CanvasRenderer>();
            renderer.cullTransparentMesh = true;
            return renderer;
        }
    }
}
