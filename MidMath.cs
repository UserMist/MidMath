using Impl = System.Runtime.CompilerServices.MethodImplAttribute;
using static System.Runtime.CompilerServices.MethodImplOptions; 

namespace MidMath
{
    public static class math
    {
        public const float degreeInRads = 0.01745329f, radsInDegrees = 1f/degreeInRads;
        public const float pi = 3.141592653589793f, tau = 2*pi;
        /// <summary>
        /// Square.
        /// </summary>
        [Impl(AggressiveInlining)] public static float sq(float a) => a * a;
        [Impl(AggressiveInlining)] public static float reciprocalSqrtEstimate(float x) => MathF.ReciprocalSqrtEstimate(x);
        [Impl(AggressiveInlining)] public static float sqrt(float x) => MathF.Sqrt(x);
        [Impl(AggressiveInlining)] public static float cos(float x) => MathF.Cos(x);
        [Impl(AggressiveInlining)] public static float sin(float x) => MathF.Sin(x);
        [Impl(AggressiveInlining)] public static float acos(float x) => MathF.Acos(x);
        [Impl(AggressiveInlining)] public static float asin(float x) => MathF.Asin(x);
        [Impl(AggressiveInlining)] public static float abs(float x) => MathF.Abs(x);
        [Impl(AggressiveInlining)] public static float sign(float x) => MathF.Sign(x);
        [Impl(AggressiveInlining)] public static float atan2(float sin, float cos) => MathF.Atan2(sin, cos);
        [Impl(AggressiveInlining)] public static float loopClamp(float x, float min, float max) => mod(x - min, max - min) + min;
        [Impl(AggressiveInlining)] public static float mod(float x, float max) => x - MathF.Floor(x / max) * max;
        [Impl(AggressiveInlining)] public static float fmod(float x, float y) => x % y;

        /// <summary>
        /// Results in value, that represents how much vector pair is orthogonal and in what plane it lies. Example: x^y = [xy], y^x = -[xy]
        /// Result is a bivector, commonly used to represent rotation inside a plane, although in 3d (pseudo)vector is also valid interpretation.
        /// </summary>
        [Impl(AggressiveInlining)] public static float3 cross(this float3 a, float3 b) => new float3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        [Impl(AggressiveInlining)] public static float cross(this float2 a, float2 b) => a.x * b.y - a.y * b.x;
        [Impl(AggressiveInlining)] public static float3 biwedge(this float3 a, float3 b) => -a.cross(b);

        #region floor
        [Impl(AggressiveInlining)] public static float floor(float a) => System.MathF.Floor(a);
        [Impl(AggressiveInlining)] public static float2 floor(float2 a) => new float2(floor(a.x), floor(a.y));
        [Impl(AggressiveInlining)] public static float3 floor(float3 a) => new float3(floor(a.x), floor(a.y), floor(a.z));
        [Impl(AggressiveInlining)] public static float4 floor(float4 a) => new float4(floor(a.x), floor(a.y), floor(a.z), floor(a.w));
        #endregion

        #region max
        [Impl(AggressiveInlining)] public static float max(float a, float b) => b > a ? b : a;
        [Impl(AggressiveInlining)] public static float2 max(float2 a, float2 b) => new float2(max(a.x, b.x), max(a.y, b.y));
        [Impl(AggressiveInlining)] public static float3 max(float3 a, float3 b) => new float3(max(a.x, b.x), max(a.y, b.y), max(a.z, b.z));
        [Impl(AggressiveInlining)] public static float4 max(float4 a, float4 b) => new float4(max(a.x, b.x), max(a.y, b.y), max(a.z, b.z), max(a.w, b.w));

        [Impl(AggressiveInlining)]
        public static float max(params float[] values)
        {
            var length = values.Length;
            if(length == 0) return 0;
            var num = values[0];
            for(int index = 1; index < length; ++index)
            {
                if(values[index] > num)
                    num = values[index];
            }
            return num;
        }
        #endregion

        #region min
        [Impl(AggressiveInlining)] public static float min(float a, float b) => b < a ? b : a;
        [Impl(AggressiveInlining)] public static float2 min(float2 a, float2 b) => new float2(min(a.x, b.x), min(a.y, b.y));
        [Impl(AggressiveInlining)] public static float3 min(float3 a, float3 b) => new float3(min(a.x, b.x), min(a.y, b.y), min(a.z, b.z));
        [Impl(AggressiveInlining)] public static float4 min(float4 a, float4 b) => new float4(min(a.x, b.x), min(a.y, b.y), min(a.z, b.z), min(a.w, b.w));

        [Impl(AggressiveInlining)]
        public static float min(params float[] values)
        {
            var length = values.Length;
            if(length == 0) return 0;
            var num = values[0];
            for(int index = 1; index < length; ++index)
            {
                if(values[index] < num)
                    num = values[index];
            }
            return num;
        }
        #endregion

        #region clamp
        [Impl(AggressiveInlining)] public static float clamp(float x, float a, float b) => min(max(x, a), b);
        [Impl(AggressiveInlining)] public static float2 clamp(float2 x, float2 a, float2 b) => min(max(x, a), b);
        [Impl(AggressiveInlining)] public static float3 clamp(float3 x, float3 a, float3 b) => min(max(x, a), b);
        [Impl(AggressiveInlining)] public static float4 clamp(float4 x, float4 a, float4 b) => min(max(x, a), b);
        #endregion

        /*
        public static void Mandelbrot(rotor2 pos, int stepLimit, out int steps, out rotor2 Z, out bool stable) {
            Z = rotor2.zero;
            stable = true;

            for(steps = 0; stable && steps < stepLimit; steps++) {
                Z = Z.then(Z).rawHalfAdd(pos);
                stable = lengthSq(Z) < sq(2);
            }
        }
        */


        #region dot
        [Impl(AggressiveInlining)] public static float dot(this float2 a, float2 b) => a.x * b.x + a.y * b.y;
        [Impl(AggressiveInlining)] public static float dot(this float3 a, float3 b) => a.x * b.x + a.y * b.y + a.z * b.z;
        [Impl(AggressiveInlining)] public static float dot(this float4 a, float4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        #endregion

        #region cosTo
        [Impl(AggressiveInlining)] public static float cosTo(this float2 a, float2 b) => a.dot(b) / (a.length * b.length);
        [Impl(AggressiveInlining)] public static float cosTo(this float3 a, float3 b) => a.dot(b) / (a.length * b.length);
        [Impl(AggressiveInlining)] public static float cosTo(this float4 a, float4 b) => a.dot(b) / (a.length * b.length);
        #endregion

        #region angleTo
        [Impl(AggressiveInlining)] public static float angleTo(this float2 a, float2 b) => acos(a.dot(b) / (a.length * b.length));
        [Impl(AggressiveInlining)] public static float angleTo(this float3 a, float3 b) => acos(a.dot(b) / (a.length * b.length));
        [Impl(AggressiveInlining)] public static float angleTo(this float4 a, float4 b) => acos(a.dot(b) / (a.length * b.length));
        #endregion

        [Impl(AggressiveInlining)] public static rotor3f rotationTo(this float3 a, float3 b) => a.twiceRotationTo(b).half;
        [Impl(AggressiveInlining)] public static rotor3f twiceRotationTo(this float3 a, float3 b) => new rotor3f(dot(a, b), cross(a, b));

        [Impl(AggressiveInlining)]
        public static void swap(ref float a, ref float b) {
            float t = a;
            a = b;
            b = t;
        }

        [Impl(AggressiveInlining)]
        public static rotor3f eulerAsRotor(this float3 rpy)
        {
            var roll  = 0.5f*rpy.x;
            var pitch = 0.5f*rpy.y;
            var yaw   = 0.5f*rpy.z;

            var p = new rotor3f(cos(roll),  new float3(sin(roll),0,0));
            var y = new rotor3f(cos(pitch), new float3(0,sin(pitch),0));
            var r = new rotor3f(cos(yaw),   new float3(0,0,sin(yaw)));
            return (p).then(y).then(r);
        }


        #region sqrt
        /// <summary>
        /// Square root.
        /// </summary>
        [Impl(AggressiveInlining)] public static float2 sqrt(float2 a) => new float2(sqrt(a.x), sqrt(a.y));
        /// <summary>
        /// Square root.
        /// </summary>
        [Impl(AggressiveInlining)] public static float3 sqrt(float3 a) => new float3(sqrt(a.x), sqrt(a.y), sqrt(a.z));
        /// <summary>
        /// Square root.
        /// </summary>
        [Impl(AggressiveInlining)] public static float4 sqrt(float4 a) => new float4(sqrt(a.x), sqrt(a.y), sqrt(a.z), sqrt(a.w));
        #endregion

        [Impl(AggressiveInlining)]
        public static float3 approach(float3 origin, float3 target, float maxStepDistance) {
            var dpos = target-origin;
            var m = maxStepDistance / dpos.length;
            return (float.IsNormal(m)) ? (dpos * min(m, 1) + origin) : (target);
        }

        #region lerp
        [Impl(AggressiveInlining)] public static float lerp(float origin, float target, float portion) => (target - origin) * portion + origin;
        [Impl(AggressiveInlining)] public static float2 lerp(float2 origin, float2 target, float portion) => (target - origin) * portion + origin;
        [Impl(AggressiveInlining)] public static float3 lerp(float3 origin, float3 target, float portion) => (target - origin) * portion + origin;
        [Impl(AggressiveInlining)] public static float4 lerp(float4 origin, float4 target, float portion) => (target - origin) * portion + origin;
        #endregion
    }

    public static class GLSL
    {
        [Impl(AggressiveInlining)] public static float2 vec2(float x, float y) => new float2(x, y);

        [Impl(AggressiveInlining)] public static float3 vec3(float x, float y, float z) => new float3(x, y, z);
        [Impl(AggressiveInlining)] public static float3 vec3(float2 xy, float z) => new float3(xy.x, xy.y, z);
        [Impl(AggressiveInlining)] public static float3 vec3(float x, float2 yz) => new float3(x, yz.x, yz.y);

        [Impl(AggressiveInlining)] public static float4 vec4(float x, float y, float z, float w) => new float4(x, y, z, w);
        [Impl(AggressiveInlining)] public static float4 vec4(float2 xy, float z, float w) => new float4(xy.x, xy.y, z, w);
        [Impl(AggressiveInlining)] public static float4 vec4(float x, float2 yz, float w) => new float4(x, yz.x, yz.y, w);
        [Impl(AggressiveInlining)] public static float4 vec4(float x, float y, float2 zw) => new float4();
        [Impl(AggressiveInlining)] public static float4 vec4(float3 xyz, float w) => new float4(xyz.x, xyz.y, xyz.z, w);
        [Impl(AggressiveInlining)] public static float4 vec4(float x, float3 yzw) => new float4(x, yzw.x, yzw.y, yzw.z);

        [Impl(AggressiveInlining)] public static float mix(float min, float max, float t) => min * (1f - t) + max * t;
        [Impl(AggressiveInlining)] public static float2 mix(float2 min, float2 max, float t) => min * (1f - t) + max * t;
        [Impl(AggressiveInlining)] public static float3 mix(float3 min, float3 max, float t) => min * (1f - t) + max * t;
        [Impl(AggressiveInlining)] public static float4 mix(float4 min, float4 max, float t) => min * (1f - t) + max * t;

        [Impl(AggressiveInlining)] public static float fma(float m, float a, float b) => m * a + b;
        [Impl(AggressiveInlining)] public static float2 fma(float2 m, float2 a, float2 b) => m * a + b;
        [Impl(AggressiveInlining)] public static float3 fma(float3 m, float3 a, float3 b) => m * a + b;
        [Impl(AggressiveInlining)] public static float4 fma(float4 m, float4 a, float4 b) => m * a + b;

        [Impl(AggressiveInlining)] public static float fract(float x) => x % 1f;
        [Impl(AggressiveInlining)] public static float2 fract(float2 x) => x % 1f;
        [Impl(AggressiveInlining)] public static float3 fract(float3 x) => x % 1f;
        [Impl(AggressiveInlining)] public static float4 fract(float4 x) => x % 1f;
    }

    public static class HLSL
    {
        [Impl(AggressiveInlining)] public static float saturate(float x) => math.clamp(x, 0, 1);
        [Impl(AggressiveInlining)] public static float2 saturate(float2 x) => math.clamp(x, float2.zero, float2.all(1));
        [Impl(AggressiveInlining)] public static float3 saturate(float3 x) => math.clamp(x, float3.zero, float3.all(1));
        [Impl(AggressiveInlining)] public static float4 saturate(float4 x) => math.clamp(x, float4.zero, float4.all(1));

        [Impl(AggressiveInlining)] public static float mad(float m, float a, float b) => m * a + b;
        [Impl(AggressiveInlining)] public static float2 mad(float2 m, float2 a, float2 b) => m * a + b;
        [Impl(AggressiveInlining)] public static float3 mad(float3 m, float3 a, float3 b) => m * a + b;
        [Impl(AggressiveInlining)] public static float4 mad(float4 m, float4 a, float4 b) => m * a + b;

        [Impl(AggressiveInlining)] public static float frac(float x) => x % 1f;
        [Impl(AggressiveInlining)] public static float2 frac(float2 x) => x % 1f;
        [Impl(AggressiveInlining)] public static float3 frac(float3 x) => x % 1f;
        [Impl(AggressiveInlining)] public static float4 frac(float4 x) => x % 1f;
    }
}

