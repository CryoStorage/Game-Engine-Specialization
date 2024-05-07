using UnityEngine;

namespace CryoStorage
{
    public static class CryoMath
    {
        //this hard coded value aligns the angle to the forward vector
        private static float angleOffset = 1.5699f;
        public static Vector3 PointOnRadius(Vector3 center, float angle, float radius = 1)
        {
            var rad = angle * Mathf.Deg2Rad;
            var xOffset = radius * Mathf.Cos(-rad + angleOffset);
            var zOffset = radius * Mathf.Sin(-rad + angleOffset);
            var result = new Vector3(center.x + xOffset, center.y, center.z + zOffset);
            return result;
        }
        
        public static Vector3 PointOnRadiusRelative(Transform center, float radius, float angle)
        {
            var rad = (angle + center.eulerAngles.y) * Mathf.Deg2Rad;
            var xOffset = radius * Mathf.Cos(-rad + angleOffset);
            var zOffset = radius * Mathf.Sin(-rad + angleOffset);
            var result = center.position + new Vector3(xOffset, 0f, zOffset);
            return result;
        }

        public static Quaternion AimAtDirection(Vector3 center, Vector3 position)
        {
            var aimDir = position - center;
            var result = Quaternion.LookRotation(aimDir);
            return result;
        }

        public static float InverseMap(float maxValue, float currentValue, float minValue)
        {
            var result = (maxValue - currentValue) / (maxValue - minValue) * (minValue - 1f) + 1f;;
            return result;
        }
        
        public static float LerpAngle(float currentAngle, float targetAngle, float angleChangeSpeed)
        {
            var result = Mathf.LerpAngle(currentAngle, targetAngle, angleChangeSpeed * Time.fixedDeltaTime);
            return result;
        }
        
        public static float InverseMapSkewed(float maxValue, float minValue, float currentValue, float skewFactor)
        {
            var inputRange = maxValue - minValue;
            var resultRange = 1.2f - 0.9f;
            var skewedValue = (currentValue - 1f) * (currentValue > 1f ? skewFactor : -skewFactor) + currentValue;
            var result = (1.2f - skewedValue) / resultRange * inputRange + maxValue;
            return result;
        }
        
        public static float AngleFromOffset(Vector2 vectorInput)
        {
            float angle = Mathf.Atan2(vectorInput.x, vectorInput.y) * Mathf.Rad2Deg;
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }
    }

    public static class CryoSteering
    {
        public static Vector3 Seek(Vector3 targetPosition, Vector3 currentPosition, Vector3 currentVelocity, float maxSpeed)
        {
            var desiredVelocity = (targetPosition - currentPosition).normalized * maxSpeed;
            var steering = desiredVelocity - currentVelocity;
            return steering;
        }
        
        public static Vector3 Flee(Vector3 targetPosition, Vector3 currentPosition, Vector3 currentVelocity, float maxSpeed)
        {
            var desiredVelocity = Seek(targetPosition, currentPosition, currentVelocity,maxSpeed) * -1;
            var steering = desiredVelocity - currentVelocity;
            return steering;
        }
        
        public static Vector3 Pursuit(Vector3 targetPosition, Vector3 targetVelocity, Vector3 currentPosition, Vector3 currentVelocity, float maxSpeed)
        {
            var distance = Vector3.Distance(currentPosition, targetPosition);
            var timeToReach = distance / maxSpeed;
            var futurePosition = targetPosition + targetVelocity * timeToReach;
            return Seek(futurePosition, currentPosition, currentVelocity, maxSpeed);
        }
    }
}