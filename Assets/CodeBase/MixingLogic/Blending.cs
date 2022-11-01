using System;
using CodeBase.Food;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.LevelManager;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.MixingLogic
{
    public class Blending
    {
        private readonly EventReferer eventReferer;
        private readonly MoveFoodToBlender moveFoodToBlender;
        private readonly LevelManager levelManager;
        private readonly Transform waterParent;


        [Inject]
        public Blending(EventReferer eventReferer, MoveFoodToBlender moveFoodToBlender, LevelManager levelManager,
            [Inject(Id = "WaterParent")] Transform waterParent)
        {
            this.eventReferer = eventReferer;
            this.moveFoodToBlender = moveFoodToBlender;
            this.levelManager = levelManager;
            this.waterParent = waterParent;
        }


        public void SubscribeToEvent()
        {
            eventReferer.OnMixedButtonClicked += StartBlending;
            eventReferer.OnLevelReset += LevelReset;
        }

        public void UnSubscribeFromEvent() =>
            eventReferer.OnMixedButtonClicked -= StartBlending;

        private void LevelReset() =>
            waterParent.transform.DOScaleY(0, 1f);

        private void StartBlending()
        {
            Color reference = levelManager.GetNextLevelColor();

            float r = 0, g = 0, b = 0;
            int count = 0;
            foreach (GameObject food in moveFoodToBlender.foodList)
            {
                r += food.GetComponent<FoodInfoStorage>().FoodMainMaterial.color.r;
                g += food.GetComponent<FoodInfoStorage>().FoodMainMaterial.color.g;
                b += food.GetComponent<FoodInfoStorage>().FoodMainMaterial.color.b;
                count++;
            }

            Color overallColor;
            if (r == 0.0f && g == 0.0f && b == 0.0f)
                overallColor = Color.white;
            else
                overallColor = new(r / count, g / count, b / count);


            Color.RGBToHSV(overallColor, out float overallH, out float overallS, out float overallV);
            Color.RGBToHSV(reference, out float referenceH, out float referenceS, out float referenceV);

            double delta =
                Math.Sqrt(
                    Math.Pow(referenceH - overallH, 2) +
                    Math.Pow(referenceS - overallS, 2) +
                    Math.Pow(referenceV - overallV, 2)) * 100;

            Material waterMaterial = waterParent.GetChild(0).GetComponent<Renderer>().material;
            var tweenSequence = DOTween.Sequence();
            tweenSequence.Join(waterParent.transform.DOScaleY(.8f, 2f));
            tweenSequence.Join(waterMaterial.DOColor(overallColor, 2f));

            int result = (int) Math.Ceiling(100 - delta);
            if (result < 0)
                result = 0;

            //Previous Color Difference calculation
            Debug.Log($"With formula {100 - CompareTwoColors(overallColor, reference) / 10}");
            //Color Difference calculation
            Debug.Log($"With HSV {100 - delta}");
            //UnSubscribeFromEvent();

            tweenSequence.OnComplete(() =>
            {
                eventReferer.BlendingFinished(result);
                //moveFoodToBlender.PrepareForNextLevel();
            });
        }

        private float CompareTwoColors(Color32 comparing, Color32 reference)
        {
            (float X, float Y, float Z) comparingXYZ = RGBtoXYZ(comparing.r, comparing.g, comparing.b);
            (float X, float Y, float Z) referenceXYZ = RGBtoXYZ(reference.r, reference.g, reference.b);

            (float L, float A, float B) comparingLAB = XYZtoLAB(comparingXYZ.X, comparingXYZ.Y, comparingXYZ.Z);
            (float L, float A, float B) referenceLAB = XYZtoLAB(referenceXYZ.X, referenceXYZ.Y, referenceXYZ.Z);


            return DeltaE(comparingLAB, referenceLAB);
        }

        private (float X, float Y, float Z) RGBtoXYZ(int red, int green, int blue)
        {
            // normalize red, green, blue values
            float rLinear = red / 255.0f;
            float gLinear = green / 255.0f;
            float bLinear = blue / 255.0f;

            // convert to a sRGB form
            float r = rLinear > 0.04045f
                ? (float) Math.Pow((rLinear + 0.055f) / 1.055f, 2.4f)
                : rLinear / 12.92f;
            float g = gLinear > 0.04045f
                ? (float) Math.Pow((gLinear + 0.055f) / 1.055f, 2.4f)
                : gLinear / 12.92f;
            float b = bLinear > 0.04045f
                ? (float) Math.Pow((bLinear + 0.055f) / 1.055f, 2.4f)
                : bLinear / 12.92f;


            r *= 100.0f;
            g *= 100.0f;
            b *= 100.0f;

            // converts
            return (
                r * 0.412453f + g * 0.357580f + b * 0.180423f,
                r * 0.212671f + g * 0.715160f + b * 0.072169f,
                r * 0.019334f + g * 0.119193f + b * 0.950227f
            );
        }

        private (float L, float A, float B) XYZtoLAB(float x, float y, float z)
        {
            (float L, float A, float B) LAB;

            x = x > .008856f ? (float) Math.Pow(x, 1.0f / 3.0f) : x * 7.787f + 16.0f / 116.0f;
            y = y > .008856f ? (float) Math.Pow(y, 1.0f / 3.0f) : y * 7.787f + 16.0f / 116.0f;
            z = z > .008856f ? (float) Math.Pow(z, 1.0f / 3.0f) : z * 7.787f + 16.0f / 116.0f;

            LAB.L = 116.0f * x - 16.0f;
            LAB.A = 500.0f * x - y;
            LAB.B = 200 * y - z;

            return LAB;
        }

        private float DeltaE((float L, float A, float B) comparingLAB, (float L, float A, float B) referenceLAB) =>
            (float) Math.Sqrt(Math.Pow(referenceLAB.L - comparingLAB.L, 2) +
                              Math.Pow(referenceLAB.A - comparingLAB.A, 2) +
                              Math.Pow(referenceLAB.B - comparingLAB.B, 2));
    }
}


// Color lime;
// ColorUtility.TryParseHtmlString("#81F800", out lime);
//
// Color.RGBToHSV(lime, out float redH, out float redS, out float redV);
// Color.RGBToHSV(overallColor, out float h, out float s, out float v);
//

// var averageRed = (redH + redS + redV) / 3;
//
// //var averageOverall = (h + redH) / 2 + (s + redS) / 2 + (v + redV) / 2;
// var averageOverall = (h + s + v) / 3;
//
// var differencePercentage = Math.Abs(averageRed - averageOverall) * 100;
//
// int result = (int) Mathf.Abs(100 - differencePercentage);
//
// Debug.Log(result);
//await UniTask.Delay(1000);