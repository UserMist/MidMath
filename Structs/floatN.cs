using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidMath
{
    /// <summary>
    /// Numeric 2-tuple.
    /// </summary>
    public struct float2
    {
        /// <summary>
        /// First value.
        /// </summary>
        public float x;
        /// <summary>
        /// Second value.
        /// </summary>
        public float y;

        #region Initializations
        public float2(float x, float y) { this.x = x; this.y = y; }
        public float2 withX(float x) => new float2(x, y);
        public float2 withY(float y) => new float2(x, y);

        public static float2 all(float a) => new float2(a, a);
        public static readonly float2 zero = default;
        public static readonly float2 unitX = new float2(1f, 0f);
        public static readonly float2 unitY = new float2(0f, 1f);
        #endregion

        #region Indexer
        public float this[int id]
        {
            get
            {
                switch(id)
                {
                    case 0: return x;
                    case 1: return y;
                    default: return float.NaN;
                }
            }
            set
            {
                switch(id)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: break;
                }
            }
        }
        #endregion

        public float2 reordered => new float2(y, x);

        #region Length and normal properties
        /// <summary>
        /// Length of a vector squared.
        /// </summary>
        public float lengthSq => x * x + y * y;
        public float length => math.sqrt(x * x + y * y);

        public float2 withLength(float value)
        {
            var ret = this;
            float m = value * math.reciprocalSqrtEstimate(x*x + y*y);
            ret.x *= m; ret.y *= m;
            return ret;
        }

        public float2 normal => this * math.reciprocalSqrtEstimate(x * x + y * y);

        /// <summary>
        /// Always returns
        /// </summary>
        public float2 safeNormal
        {
            get
            {
                var lSq = lengthSq;
                return lSq > 0 ? this * math.reciprocalSqrtEstimate(lSq) : unitX;
            }
        }

        public float2 orthorotated => new float2(-y, x);
        #endregion

        public bool hasNaN => float.IsNaN(x) || float.IsNaN(y);
        public override string ToString() => $"({x}; {y})";

        #region Operator overloads
        public static float3 operator +(float2 a) => a;
        public static float2 operator -(float2 a) => new float2(-a.x, -a.y);

        public static float2 operator +(float2 a, float2 b) => new float2(a.x + b.x, a.y + b.y);
        public static float2 operator -(float2 a, float2 b) => new float2(a.x - b.x, a.y - b.y);
        public static float2 operator *(float2 a, float2 b) => new float2(a.x * b.x, a.y * b.y);
        public static float2 operator /(float2 a, float2 b) => new float2(a.x / b.x, a.y / b.y);
        public static float2 operator %(float2 a, float2 b) => new float2(a.x % b.x, a.y % b.y);

        public static float2 operator *(float2 a, float b) => new float2(a.x * b, a.y * b);
        public static float2 operator /(float2 a, float b) => a * (1f / b);
        public static float2 operator %(float2 a, float b) => new float2(a.x % b, a.y % b);

        public static float2 operator +(float2 a, float b) => new float2(a.x + b, a.y + b);
        public static float2 operator -(float2 a, float b) => new float2(a.x - b, a.y - b);
        public static float2 operator +(float a, float2 b) => new float2(a + b.x, a + b.y);
        public static float2 operator -(float a, float2 b) => new float2(a - b.x, a - b.y);

        public static float2 operator *(float a, float2 b) => new float2(a * b.x, a * b.y);
        public static float2 operator /(float a, float2 b) => new float2(a / b.x, a / b.y);
        #endregion

        #region Linear transformations
        public float2 transformedTo(matrix2f b) => x * b.x + y * b.y;

        public float2 transformedTo(resizer2f b)
        {
            return new float2(
                x * b.xx + y * b.xy,
                x * b.xy + y * b.yy
            );
        }

        public float2 transformedTo(rotor2f b)
        {
            return new float2(
                x * b.s - y * b.bivec,
                y * b.s + x * b.bivec
            );
        }

        public float2 transformedToAntirotated(matrix2f b) => new float2(this.dot(b.x), this.dot(b.y));
        #endregion
    }


    /// <summary>
    /// Numeric 3-tuple. 
    /// Often used to represent vectors (oriented lengths) or 3d bivectors/pseudovectors (oriented angles), but can be really used for any tiny collection of numbers.
    /// </summary>
    [System.Serializable]
    public struct float3
    {
        /// <summary>
        /// First value (3d bivector alias is "yz").
        /// </summary>
        public float x;
        /// <summary>
        /// Second value (3d bivector alias is "zx").
        /// </summary>
        public float y;
        /// <summary>
        /// Third value (3d bivector alias is "xy").
        /// </summary>
        public float z;

        #region Initializations
        public float3(float x, float y, float z = 0f) { this.x = x; this.y = y; this.z = z; }
        public float3 withX(float x) => new float3(x, y, z);
        public float3 withY(float y) => new float3(x, y, z);
        public float3 withZ(float z) => new float3(x, y, z);

        public static float3 all(float a) => new float3(a, a, a);
        public static readonly float3 zero = default;
        public static readonly float3 unitX = new float3(1f, 0f, 0f);
        public static readonly float3 unitY = new float3(0f, 1f, 0f);
        public static readonly float3 unitZ = new float3(0f, 0f, 1f);
        #endregion

        #region Indexer
        public float this[int id]
        {
            get
            {
                switch(id)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: return float.NaN;
                }
            }
            set
            {
                switch(id)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: break;
                }
            }
        }
        #endregion

        public float3 reordered(int id0, int id1, int id2) => new float3(this[id0], this[id1], this[id2]);

        #region Length and normal properties
        /// <summary>
        /// Length of a vector squared.
        /// </summary>
        public float lengthSq => x * x + y * y + z * z;
        public float length => math.sqrt(x * x + y * y + z * z);

        public float3 withLength(float value)
        {
            var ret = this;
            float m = value * math.reciprocalSqrtEstimate(x*x + y*y + z*z);
            ret.x *= m; ret.y *= m; ret.z *= m;
            return ret;
        }

        public float3 normal => this * math.reciprocalSqrtEstimate(x * x + y * y + z * z);

        /// <summary>
        /// Always returns
        /// </summary>
        public float3 safeNormal
        {
            get
            {
                var lSq = lengthSq;
                return lSq > 0 ? this * math.reciprocalSqrtEstimate(lSq) : unitX;
            }
        }

        public float3 orthoNormal
        {
            get => x == 0 && y == 0 ? unitX : this.cross(unitZ).normal;
            set => this = this.cross(value).cross(value).normal * length;
        }
        #endregion

        public bool hasNaN => float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z);
        public override string ToString() => $"({x}; {y}; {z})";

        #region Operator overloads
        public static float3 operator +(float3 a) => a;
        public static float3 operator -(float3 a) => new float3(-a.x, -a.y, -a.z);

        public static float3 operator +(float3 a, float3 b) => new float3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static float3 operator -(float3 a, float3 b) => new float3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static float3 operator *(float3 a, float3 b) => new float3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static float3 operator /(float3 a, float3 b) => new float3(a.x / b.x, a.y / b.y, a.z / b.z);
        public static float3 operator %(float3 a, float3 b) => new float3(a.x % b.x, a.y % b.y, a.z % b.z);

        public static float3 operator *(float3 a, float b) => new float3(a.x * b, a.y * b, a.z * b);
        public static float3 operator /(float3 a, float b) => a * (1f / b);
        public static float3 operator %(float3 a, float b) => new float3(a.x % b, a.y % b, a.z % b);

        public static float3 operator +(float3 a, float b) => new float3(a.x + b, a.y + b, a.z + b);
        public static float3 operator -(float3 a, float b) => new float3(a.x - b, a.y - b, a.z - b);
        public static float3 operator +(float a, float3 b) => new float3(a + b.x, a + b.y, a + b.z);
        public static float3 operator -(float a, float3 b) => new float3(a - b.x, a - b.y, a - b.z);

        public static float3 operator *(float a, float3 b) => new float3(a * b.x, a * b.y, a * b.z);
        public static float3 operator /(float a, float3 b) => new float3(a / b.x, a / b.y, a / b.z);

        public static implicit operator float3(float2 a) => new float3(a.x, a.y);
        public static explicit operator float2(float3 a) => new float2(a.x, a.y);
        #endregion

        #region Linear transformations
        public float3 transformedTo(matrix3f b) => x * b.x + y * b.y + z * b.z;

        public float3 transformedTo(resizer3f b)
        {
            return new float3(
                x * b.xx + y * b.xy + z * b.zx,
                x * b.xy + y * b.yy + z * b.yz,
                x * b.zx + y * b.yz + z * b.zz
            );
        }

        public float3 transformedToAntirotated(matrix3f b) => new float3(this.dot(b.x), this.dot(b.y), this.dot(b.z));

        public float3 transformedTo(rotor3f b)
        {
            var a = this;

            //naive
            var _xyz = -(b._yz*a.x + b._zx*a.y + b._xy*a.z);
            var _x = b.s*a.x + b._zx*a.z - b._xy*a.y;
            var _y = b.s*a.y + b._xy*a.x - b._yz*a.z;
            var _z = b.s*a.z + b._yz*a.y - b._zx*a.x;

            return new float3(
                _x * b.s + _z * b._zx - _y * b._xy - _xyz * b._yz,
                _y * b.s + _x * b._xy - _z * b._yz - _xyz * b._zx,
                _z * b.s + _y * b._yz - _x * b._zx - _xyz * b._xy
            );
        }
        #endregion

        #region Linear transformations of an aligned resizer
        /// <summary>
        /// Turns (ang. momentum -> ang. velocity) transformation into (force -> ang. acceleration) at a specified point relative to mass center.
        /// </summary>
        public resizer3f asResizerOutsideTorqueSpace(float3 arm)
        {
            var a = this; var b = arm;

            //15* 3+ 1-
            float z_ = b.z*b.z, y_ = b.y*b.y, x_ = b.x*b.x;
            return new resizer3f()
            {
                vec = new float3(a.y * z_ + y_ * a.z, a.x * z_ + x_ * a.z, a.x * y_ + a.y * x_),
                bivec = new float3(a.x * b.y * b.z, b.x * a.y * b.z, b.x * b.y * a.z)
            };
        }

        /// <summary>
        /// Transforms aligned resizer from space B to space A when given rotational transformation (A -> B). Results in non-aligned resizer.
        /// </summary>
        public resizer3f asResizerOutsideRotation(rotor3f r)
        {
            float3 V = this;
            float s = r.s, x = r.bivec.x, y = r.bivec.y, z = r.bivec.z;
            //we represent rotor as a matrix

            float xx_ = x*x, yy_ = y*y, zz_ = z*z;
            float xx = 1-2*(yy_+zz_), yy = 1-2*(xx_+zz_), zz = 1-2*(xx_+yy_); //diagonal cells

            float xy0 = 2*x*y, xy1 = 2*s*z;  //sum terms at non-diagonal cells
            float xz0 = 2*x*z, xz1 = -2*s*y;
            float yz0 = 2*y*z, yz1 = 2*s*x;

            float xyA = xy0+xy1, xyB = xy0-xy1; //non-diagonal cells
            float xzA = xz0+xz1, xzB = xz0-xz1;
            float yzA = yz0+yz1, yzB = yz0-yz1;

            float a = V.x, b = V.y, c = V.z;

            //find sandwich result
            return new resizer3f()
            {
                vec = new float3(
                    a * xx * xx + b * xyB * xyB + c * xzB * xzB,
                    a * xyA * xyA + b * yy * yy + c * yzB * yzB,
                    a * xzA * xzA + b * yzA * yzA + c * zz * zz
                ),
                bivec = new float3(
                    a * xyA * xzA + b * yy * yzA + c * zz * yzB, //yz|zy
                    a * xx * xzA + b * xyB * yzA + c * zz * xzB, //zx|xz
                    a * xx * xyA + b * yy * xyB + c * xzB * yzB  //xy|yx
                )
            };
        }

        public resizer3f asResizerInsideRotation(rotor3f m) => asResizerOutsideRotation(m.antirotary);

        public resizer3f asResizerInsideRotation(matrix3f m)
        {
            return new resizer3f()
            {
                vec = new float3()
                {
                    x = x * m.xx * m.xx + y * m.yx * m.yx + z * m.zx * m.zx,
                    y = x * m.xy * m.yx + y * m.yy * m.yy + z * m.zy * m.zy,
                    z = x * m.xz * m.zx + y * m.yz * m.yz + z * m.zz * m.zz
                },
                bivec = new float3()
                {
                    x = x * m.xy * m.xz + y * m.yy * m.yz + z * m.zz * m.zy, //yz|zy
                    y = x * m.xx * m.xz + y * m.yx * m.yz + z * m.zz * m.zx, //zx|xz
                    z = x * m.xx * m.xy + y * m.yy * m.yx + z * m.zx * m.zy  //xy|yx
                }
            };
        }

        public resizer3f asResizerOutsideRotation(matrix3f m) => asResizerInsideRotation(m.antirotary);
        #endregion
    }

    /// <summary>
    /// Numeric 4-tuple.
    /// </summary>
    public struct float4
    {
        /// <summary>
        /// First value.
        /// </summary>
        public float x;
        /// <summary>
        /// Second value.
        /// </summary>
        public float y;
        /// <summary>
        /// Third value.
        /// </summary>
        public float z;
        /// <summary>
        /// Fourth value.
        /// </summary>
        public float w;

        #region Initializations
        public float4(float x, float y, float z = 0f, float w = 0f) { this.x = x; this.y = y; this.z = z; this.w = w; }
        public float4 withX(float x) => new float4(x, y, z, w);
        public float4 withY(float y) => new float4(x, y, z, w);
        public float4 withZ(float z) => new float4(x, y, z, w);
        public float4 withW(float w) => new float4(x, y, z, w);

        public static float4 all(float a) => new float4(a, a, a, a);
        public static readonly float4 zero = default;
        public static readonly float4 unitX = new float4(1f, 0f, 0f, 0f);
        public static readonly float4 unitY = new float4(0f, 1f, 0f, 0f);
        public static readonly float4 unitZ = new float4(0f, 0f, 1f, 0f);
        public static readonly float4 unitW = new float4(0f, 0f, 0f, 1f);
        #endregion

        #region Indexer
        public float this[int id]
        {
            get
            {
                switch(id)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: return float.NaN;
                }
            }
            set
            {
                switch(id)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: break;
                }
            }
        }
        #endregion

        public float4 reordered(int id0, int id1, int id2, int id3) => new float4(this[id0], this[id1], this[id2], this[id3]);

        #region Length and normal properties
        /// <summary>
        /// Length of a vector squared.
        /// </summary>
        public float lengthSq => x * x + y * y + z * z + w * w;
        public float length => math.sqrt(x * x + y * y + z * z + w * w);

        public float4 withLength(float value)
        {
            var ret = this;
            float m = value * math.reciprocalSqrtEstimate(x*x + y*y + z*z + w*w);
            ret.x *= m; ret.y *= m; ret.z *= m; ret.w *= m;
            return ret;
        }

        public float4 normal => this * math.reciprocalSqrtEstimate(x * x + y * y + z * z + w * w);

        /// <summary>
        /// Always returns
        /// </summary>
        public float4 safeNormal
        {
            get
            {
                var lSq = lengthSq;
                return lSq > 0 ? this * math.reciprocalSqrtEstimate(lSq) : unitX;
            }
        }
        #endregion

        public bool hasNaN => float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z) || float.IsNaN(w);
        public override string ToString() => $"({x}; {y}; {z}; {w})";

        #region Operator overloads
        public static float4 operator +(float4 a) => a;
        public static float4 operator -(float4 a) => new float4(-a.x, -a.y, -a.z, -a.w);

        public static float4 operator +(float4 a, float4 b) => new float4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static float4 operator -(float4 a, float4 b) => new float4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        public static float4 operator *(float4 a, float4 b) => new float4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        public static float4 operator /(float4 a, float4 b) => new float4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        public static float4 operator %(float4 a, float4 b) => new float4(a.x % b.x, a.y % b.y, a.z % b.z, a.w % b.w);

        public static float4 operator *(float4 a, float b) => new float4(a.x * b, a.y * b, a.z * b, a.w % b);
        public static float4 operator /(float4 a, float b) => a * (1f / b);
        public static float4 operator %(float4 a, float b) => new float4(a.x % b, a.y % b, a.z % b, a.w % b);

        public static float4 operator -(float4 a, float b) => new float4(a.x - b, a.y - b, a.z - b, a.w - b);
        public static float4 operator +(float4 a, float b) => new float4(a.x + b, a.y + b, a.z + b, a.w + b);
        public static float4 operator +(float a, float4 b) => new float4(a + b.x, a + b.y, a + b.z, a + b.w);
        public static float4 operator -(float a, float4 b) => new float4(a - b.x, a - b.y, a - b.z, a - b.w);

        public static float4 operator *(float a, float4 b) => new float4(a * b.x, a * b.y, a * b.z, a * b.w);
        public static float4 operator /(float a, float4 b) => new float4(a / b.x, a / b.y, a / b.z, a / b.w);

        public static implicit operator float4(float2 a) => new float4(a.x, a.y);
        public static implicit operator float4(float3 a) => new float4(a.x, a.y);
        public static explicit operator float2(float4 a) => new float2(a.x, a.y);
        public static explicit operator float3(float4 a) => new float3(a.x, a.y, a.z);
        #endregion

        #region Linear transformations
        public float3 transformedTo(matrix3f b) => x * b.x + y * b.y + z * b.z;

        public float3 transformedTo(resizer3f b)
        {
            return new float3(
                x * b.xx + y * b.xy + z * b.zx,
                x * b.xy + y * b.yy + z * b.yz,
                x * b.zx + y * b.yz + z * b.zz
            );
        }

        public float3 transformedToAntirotated(matrix3f b) => new float3(this.dot(b.x), this.dot(b.y), this.dot(b.z));

        public float3 transformedTo(rotor3f b)
        {
            var a = this;

            //naive
            var _xyz = -(b._yz*a.x + b._zx*a.y + b._xy*a.z);
            var _x = b.s*a.x + b._zx*a.z - b._xy*a.y;
            var _y = b.s*a.y + b._xy*a.x - b._yz*a.z;
            var _z = b.s*a.z + b._yz*a.y - b._zx*a.x;

            return new float3(
                _x * b.s + _z * b._zx - _y * b._xy - _xyz * b._yz,
                _y * b.s + _x * b._xy - _z * b._yz - _xyz * b._zx,
                _z * b.s + _y * b._yz - _x * b._zx - _xyz * b._xy
            );
        }
        #endregion
    }

    public struct float6
    {
        //roll.roll roll.pitch roll.yaw pitch yaw0 yaw1
        //yz        xy         zx       xw    yw   zw
        float e0, e1, e2, e3, e4, e5;
    }
}
