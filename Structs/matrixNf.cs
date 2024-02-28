namespace MidMath
{
    /// <summary>
    /// Transformation replacing unit quantities of space.
    /// </summary>
    public struct matrix2f
    {
        public float2 x, y;
        public float xx => x.x; public float xy => x.y;
        public float yx => y.x; public float yy => y.y;

        #region Initializations
        public matrix2f(float2 x, float2 y) { this.x = x; this.y = y; }
        public matrix2f withX(float2 x) => new matrix2f(x, y);
        public matrix2f withY(float2 y) => new matrix2f(x, y);

        public static matrix2f fromScalar(float a) => new matrix2f(new float2(a, 0), new float2(0, a));
        public static matrix2f fromSize(float2 a) => new matrix2f(new float2(a.x, 0), new float2(0, a.y));
        public static readonly matrix2f zero = default;
        public static readonly matrix2f identity = new matrix2f(float2.unitX, float2.unitY);
        #endregion

        #region Indexer
        public float2 this[int id]
        {
            get
            {
                switch(id)
                {
                    case 0: return x;
                    case 1: return y;
                    default: return new float2(float.NaN, float.NaN);
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

        public override string ToString() => $"[{x};  {y}]";

        #region Volume, antirotation, inversion, double transformation
        /// <summary>
        /// Commonly referred to as determinant.
        /// </summary>
        public float area => (x).cross(y);

        /// <summary>
        /// Commonly referred to as transpose.
        /// </summary>
        public matrix2f antirotary
        {
            get => new matrix2f(new float2(x.x, y.x), new float2(x.y, y.y));
            set
            {
                matrix2f v = value;
                x = new float2(v.x.x, v.y.x);
                y = new float2(v.x.y, v.y.y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public matrix2f inverted => new matrix2f(new float2(yy, -xy), new float2(-yx, xx)).antirotary * (1f / area);

        public resizer2f twice
        {
            get
            {
                return new resizer2f()
                {
                    vec = new float2(xx * xx + yx * yx, yy * yy + xy * xy),
                    bivec = xx * xy + yy * yx
                };
            }
        }

        #endregion

        #region Transformation combining
        public matrix2f then(matrix2f b) => new matrix2f(x.transformedTo(b), y.transformedTo(b));
        public matrix2f then(resizer2f b) => new matrix2f(x.transformedTo(b), y.transformedTo(b));
        public matrix2f then(rotor2f b) => new matrix2f(x.transformedTo(b), y.transformedTo(b));
        #endregion

        #region Operator overloads
        public static matrix2f operator +(matrix2f a) => a;
        public static matrix2f operator -(matrix2f a) => new matrix2f(-a.x, -a.y);

        public static matrix2f operator *(matrix2f a, float b) => new matrix2f(a.x * b, a.y * b);
        public static matrix2f operator *(float a, matrix2f b) => new matrix2f(b.x * a, b.y * a);
        public static matrix2f operator +(matrix2f a, float b) => new matrix2f(a.x.withX(a.x.x + b), a.y.withY(a.y.y + b));
        public static matrix2f operator +(float a, matrix2f b) => new matrix2f(b.x.withX(b.x.x + a), b.y.withY(b.y.y + a));
        public static matrix2f operator -(matrix2f a, float b) => new matrix2f(a.x.withX(a.x.x - b), a.y.withY(a.y.y - b));
        public static matrix2f operator -(float a, matrix2f b) => new matrix2f(b.x.withX(b.x.x - a), b.y.withY(b.y.y - a));
        public static matrix2f operator /(matrix2f a, float b) { var inv = 1f/b; return new matrix2f(a.x * inv, a.y * inv); }

        public static explicit operator float2(matrix2f a) => new float2(a.xx, a.yy);
        public static explicit operator resizer2f(matrix2f a) => new resizer2f(new float2(a.xx, a.yy), a.xy);
        public static explicit operator rotor2f(matrix2f a) => new rotor2f(a.xx, a.xy);
        #endregion
    }

    /// <summary>
    /// Transformation replacing unit quantities of space.
    /// </summary>
    public struct matrix3f
    {
        public float3 x, y, z;
        public float xx => x.x; public float xy => x.y; public float xz => x.z;
        public float yx => y.x; public float yy => y.y; public float yz => y.z;
        public float zx => z.x; public float zy => z.y; public float zz => z.z;

        #region Initializations
        public matrix3f(float3 x, float3 y, float3 z) { this.x = x; this.y = y; this.z = z; }
        public matrix3f withX(float3 x) => new matrix3f(x, y, z);
        public matrix3f withY(float3 y) => new matrix3f(x, y, z);
        public matrix3f withZ(float3 z) => new matrix3f(x, y, z);

        public static matrix3f fromScalar(float a) => new matrix3f(new float3(a, 0, 0), new float3(0, a, 0), new float3(0, 0, a));
        public static matrix3f fromSize(float3 a) => new matrix3f(new float3(a.x, 0, 0), new float3(0, a.y, 0), new float3(0, 0, a.z));
        public static readonly matrix3f zero = default;
        public static readonly matrix3f identity = new matrix3f(float3.unitX, float3.unitY, float3.unitZ);
        #endregion

        #region Indexer
        public float3 this[int id]
        {
            get
            {
                switch(id)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: return new float3(float.NaN, float.NaN, float.NaN);
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

        public override string ToString() => $"[{x};  {y};  {z}]";

        #region Volume, antirotation, inversion, double transformation
        /// <summary>
        /// Commonly referred to as determinant.
        /// </summary>
        public float volume => (x).cross(y).dot(z);

        /// <summary>
        /// Commonly referred to as transpose.
        /// </summary>
        public matrix3f antirotary
        {
            get => new matrix3f(new float3(x.x, y.x, z.x), new float3(x.y, y.y, z.y), new float3(x.z, y.z, z.z));
            set
            {
                matrix3f v = value;
                x = new float3(v.x.x, v.y.x, v.z.x);
                y = new float3(v.x.y, v.y.y, v.z.y);
                z = new float3(v.x.z, v.y.z, v.z.z);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public matrix3f inverted => new matrix3f(y.cross(z), z.cross(x), x.cross(y)).antirotary * (1f / volume);

        public resizer3f twice
        {
            get
            {
                return new resizer3f()
                {
                    vec = new float3(xx * xx + yx * yx + zx * zx, yy * yy + xy * xy + zy * zy, zz * zz + yz * yz + xz * xz),
                    bivec = new float3(xy * xz + yy * yz + zz * zy, yx * yz + xx * xz + zz * zx, zx * zy + xx * xy + yy * yx)
                };
            }
        }

        #endregion

        #region Transformation combining
        public matrix3f then(matrix3f b) => new matrix3f(x.transformedTo(b), y.transformedTo(b), z.transformedTo(b));
        public matrix3f then(resizer3f b) => new matrix3f(x.transformedTo(b), y.transformedTo(b), z.transformedTo(b));
        public matrix3f then(rotor3f b) => new matrix3f(x.transformedTo(b), y.transformedTo(b), z.transformedTo(b));
        #endregion

        public void asResizerThenRotation(out resizer3f resizer, out matrix3f ortho)
        {
            twice.asSizeAndRotation(out float3 sizeSq, out matrix3f rotation);
            float3 size = math.sqrt(sizeSq);
            resizer = size.asResizerInsideRotation(rotation);
            var invResizer = (1f/size).asResizerInsideRotation(rotation); //var invRes = res.inverted;
            ortho = this.then(invResizer);
        }

        #region Operator overloads
        public static matrix3f operator +(matrix3f a) => a;
        public static matrix3f operator -(matrix3f a) => new matrix3f(-a.x, -a.y, -a.z);

        public static matrix3f operator *(matrix3f a, float b) => new matrix3f(a.x * b, a.y * b, a.z * b);
        public static matrix3f operator *(float a, matrix3f b) => new matrix3f(b.x * a, b.y * a, b.z * a);
        public static matrix3f operator +(matrix3f a, float b) => new matrix3f(a.x.withX(a.x.x + b), a.y.withY(a.y.y + b), a.z.withZ(a.z.z + b));
        public static matrix3f operator +(float a, matrix3f b) => new matrix3f(b.x.withX(b.x.x + a), b.y.withY(b.y.y + a), b.z.withZ(b.z.z + a));
        public static matrix3f operator -(matrix3f a, float b) => new matrix3f(a.x.withX(a.x.x - b), a.y.withY(a.y.y - b), a.z.withZ(a.z.z - b));
        public static matrix3f operator -(float a, matrix3f b) => new matrix3f(b.x.withX(b.x.x - a), b.y.withY(b.y.y - a), b.z.withZ(b.z.z - a));
        public static matrix3f operator /(matrix3f a, float b) { var inv = 1f/b; return new matrix3f(a.x * inv, a.y * inv, a.z * inv); }

        public static explicit operator float3(matrix3f a) => new float3(a.xx, a.yy, a.zz);
        public static explicit operator resizer3f(matrix3f a) => new resizer3f(new float3(a.xx, a.yy, a.zz), new float3(a.yz, a.zx, a.xy));
        public static explicit operator rotor3f(matrix3f a)
        {
            var firstRotation = (float3.unitX).rotationTo(a.x);
            return firstRotation.then((float3.unitY).transformedTo(firstRotation).rotationTo(a.y));
        }
        #endregion
    }
}
