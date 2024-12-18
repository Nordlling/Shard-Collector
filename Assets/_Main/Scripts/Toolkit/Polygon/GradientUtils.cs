using UnityEngine;

namespace _Main.Scripts.Toolkit.Polygon
{
    public static class GradientUtils
    {
        private static readonly MaterialPropertyBlock PropertyBlock = new();
        private static readonly int TopLeftColor = Shader.PropertyToID("_TopLeftColor");
        private static readonly int TopRightColor = Shader.PropertyToID("_TopRightColor");
        private static readonly int BottomLeftColor = Shader.PropertyToID("_BottomLeftColor");
        private static readonly int BottomRightColor = Shader.PropertyToID("_BottomRightColor");
        
        public static MaterialPropertyBlock GetChangeColorPropertyBlock(Material shapeMaterial, Vector3 patternPosition, Vector3 patternSize, Vector3 shapePosition, Vector3 shapeSize)
        {
            Vector3 patternMinPosition = patternPosition - patternSize / 2;
            
            Vector3 shapeMinPosition = shapePosition - shapeSize / 2;
            Vector3 shapeMaxPosition = shapePosition + shapeSize / 2;

            Vector3 shapeMinDistance = new Vector2(Mathf.Abs(patternMinPosition.x - shapeMinPosition.x), Mathf.Abs(patternMinPosition.y - shapeMinPosition.y));
            Vector3 shapeMaxDistance = new Vector2(Mathf.Abs(patternMinPosition.x - shapeMaxPosition.x), Mathf.Abs(patternMinPosition.y - shapeMaxPosition.y));
            
            Vector3 shapeMinRelativeValue = new Vector3(shapeMinDistance.x / patternSize.x, shapeMinDistance.y / patternSize.y);
            Vector3 shapeMaxRelativeValue = new Vector3(shapeMaxDistance.x / patternSize.x, shapeMaxDistance.y / patternSize.y);
            
            Color topLeftColor = shapeMaterial.GetColor(TopLeftColor);
            Color topRightColor = shapeMaterial.GetColor(TopRightColor);
            Color bottomLeftColor = shapeMaterial.GetColor(BottomLeftColor);
            Color bottomRightColor = shapeMaterial.GetColor(BottomRightColor);

            PropertyBlock.Clear();
            SetColorToPropertyBlock(TopLeftColor, shapeMinRelativeValue.x, shapeMaxRelativeValue.y,
                topLeftColor, topRightColor, bottomLeftColor, bottomRightColor);
            SetColorToPropertyBlock(TopRightColor, shapeMaxRelativeValue.x, shapeMaxRelativeValue.y, 
                topLeftColor, topRightColor, bottomLeftColor, bottomRightColor);
            SetColorToPropertyBlock(BottomLeftColor, shapeMinRelativeValue.x, shapeMinRelativeValue.y,
                topLeftColor, topRightColor, bottomLeftColor, bottomRightColor);
            SetColorToPropertyBlock(BottomRightColor, shapeMaxRelativeValue.x, shapeMinRelativeValue.y,
                topLeftColor, topRightColor, bottomLeftColor, bottomRightColor);
            
            return PropertyBlock;
        }

        private static void SetColorToPropertyBlock(int materialParam, float relativePositionX, float relativePositionY, 
            Color topLeftColor, Color topRightColor, Color bottomLeftColor, Color bottomRightColor)
        {
            Color bottomColor = Color.Lerp(bottomLeftColor, bottomRightColor, relativePositionX);
            Color topColor = Color.Lerp(topLeftColor, topRightColor, relativePositionX);
            Color color = Color.Lerp(bottomColor, topColor, relativePositionY);
            PropertyBlock.SetColor(materialParam, color);
        }
        
    }
}