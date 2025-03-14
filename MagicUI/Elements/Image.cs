﻿using MagicUI.Core;
using UnityEngine;
using UImage = UnityEngine.UI.Image;

namespace MagicUI.Elements
{
    /// <summary>
    /// A simple image element
    /// </summary>
    public sealed class Image : ArrangableElement, IGameObjectWrapper
    {
        private readonly GameObject imgObj;
        private readonly UImage img;
        private readonly RectTransform tx;

        /// <inheritdoc/>
        public GameObject GameObject => imgObj;

        private float width;
        /// <summary>
        /// The desired width of the image; it will be scaled as needed
        /// </summary>
        public float Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    width = value;
                    InvalidateMeasure();
                }
            }
        }

        private float height;
        /// <summary>
        /// The desired height of the image; it will be scaled as needed
        /// </summary>
        public float Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    height = value;
                    InvalidateMeasure();
                }
            }
        }

        private Color color = Color.white;
        /// <summary>
        /// A color to apply over top of the image
        /// </summary>
        public Color Tint
        {
            get => color;
            set
            {
                if (color != value)
                {
                    color = value;
                    InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Creates an image
        /// </summary>
        /// <param name="onLayout">The layout root to draw the image on</param>
        /// <param name="sprite">The sprite to use to render the image</param>
        /// <param name="name">The name of the image element</param>
        public Image(LayoutRoot onLayout, Sprite sprite, string name = "New Image") : base(onLayout, name) 
        {
            imgObj = new GameObject(name);
            imgObj.AddComponent<CanvasRenderer>();

            Vector2 size = sprite.textureRect.size;
            Vector2 pos = UI.UnityScreenPosition(new Vector2(0, 0), size);
            tx = imgObj.AddComponent<RectTransform>();
            tx.sizeDelta = size;
            tx.anchorMin = pos;
            tx.anchorMax = pos;
            width = size.x;
            height = size.y;

            img = imgObj.AddComponent<UImage>();
            img.sprite = sprite;
            img.color = color;
            if (sprite.border != Vector4.zero)
            {
                img.type = UImage.Type.Sliced;
            }

            imgObj.transform.SetParent(onLayout.Canvas.transform, false);
            // hide the GO until the first arrange cycle takes control
            imgObj.SetActive(false);
        }

        /// <inheritdoc/>
        protected override Vector2 MeasureOverride()
        {
            return new Vector2(width, height);
        }

        /// <inheritdoc/>
        protected override void ArrangeOverride(Vector2 alignedTopLeftCorner)
        {
            tx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            tx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            img.color = color;

            Vector2 pos = UI.UnityScreenPosition(alignedTopLeftCorner, ContentSize);
            tx.anchorMin = pos;
            tx.anchorMax = pos;

            imgObj.SetActive(IsEffectivelyVisible);
        }

        /// <inheritdoc/>
        protected override void DestroyOverride()
        {
            Object.Destroy(imgObj);
        }
    }
}
