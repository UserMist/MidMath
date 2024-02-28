namespace MidMath
{
    /// <summary>
    /// Transformation resizing space along some 2 orthogonal directions. 
    /// Commonly known as a symmetric matrix.
    /// </summary>
    public struct resizer2f
    {
        public float2 vec;
        public float bivec;

        public float xx => vec.x;
        public float yy => vec.y;
        public float xy => bivec;
        public float yx => bivec;

        public float2 x => new float2(xx, xy);
        public float2 y => new float2(yx, yy);

        #region Initializations
        public resizer2f(float2 size)
        {
            this.vec = size;
            this.bivec = 0;
        }

        public resizer2f(float2 main, float off)
        {
            this.vec = main;
            this.bivec = off;
        }

        public static readonly resizer2f zero = default;
        public static readonly resizer2f identity = new resizer2f(float2.all(1), 0);
        public static resizer2f fromScalar(float a) => new resizer2f(float2.all(a), 0);
        public static resizer2f fromArrowResizer(float2 a, float scale)
        {
            float k = scale-1;
            return new resizer2f(
                new float2(a.x * a.x * k + 1, a.y * a.y * k + 1),
                k * a.x * a.y
            );
        }
        #endregion

        public override string ToString() => $"[{x};  {y}]";

        /// <summary>
        /// Transformation turning force into resized torque multiplied by arm length. In other words, results in force -> tangential velocity.
        /// </summary>
        public float outsideTorqueSpace(float2 arm)
        {
            float r = -arm.y, t = arm.x;
            float ar_ty = bivec*r + t*vec.y, at_rx = bivec*t + r*t;
            return r * at_rx + t * ar_ty;
        }

        //works flawlessly
        public resizer2f inverted
        {
            get
            {
                var invDet = 1f / area;
                return new resizer2f()
                {
                    vec = new float2(invDet * yy, invDet * xx),
                    bivec = -invDet * xy
                };
            }
        }

        /// <summary>
        /// Results in one of multiple possible aligned resizers. Commonly is referred to as diagonalization.
        /// </summary>
        public float2 size
        {
            get
            {
                if(xy == 0) return vec;
                var a = xx+yy;
                var b = math.sqrt(a*a - 4*area);
                return new float2(0.5f * (a + b), 0.5f * (a - b));
            }
        }

        /// <summary>
        /// Separates resizer into a size and rotation.
        /// Can be used to easily make Tissot's indicatrix. 
        /// </summary>
        public void asSizeAndRotation(out float2 size, out matrix2f rotation)
        {
            size = this.size;
            rotation = matrix2f.identity;
            if(size.x == size.y) return;

            var m = this - size.x;
            rotation.x = (m.x.lengthSq > m.y.lengthSq)? m.x : m.y;
            rotation.y = new float2(-rotation.yy, rotation.xx);

            var v = area;
            if(!float.IsNaN(v)) rotation *= math.sign(v);
        }

        public matrix2f then(resizer2f b)
        {
            return ((matrix2f) this).then(b);
        }

        public matrix2f then(rotor2f b)
        {
            return ((matrix2f) this).then(b);
        }

        public static resizer2f operator *(resizer2f a, float b) => new resizer2f { vec = a.vec * b, bivec = a.bivec * b };
        public static resizer2f operator *(float b, resizer2f a) => new resizer2f { vec = a.vec * b, bivec = a.bivec * b };
        public static resizer2f operator +(resizer2f a, resizer2f b) => new resizer2f { vec = a.vec + b.vec, bivec = a.bivec + b.bivec };
        public static resizer2f operator -(resizer2f a, resizer2f b) => new resizer2f { vec = a.vec - b.vec, bivec = a.bivec - b.bivec };
        public static resizer2f operator +(resizer2f a, float b) => new resizer2f { vec = a.vec + b, bivec = a.bivec };
        public static resizer2f operator -(resizer2f a, float b) => new resizer2f { vec = a.vec - b, bivec = a.bivec };
        public static resizer2f operator +(float a, resizer2f b) => new resizer2f { vec = b.vec + a, bivec = b.bivec };
        public static resizer2f operator -(float a, resizer2f b) => new resizer2f { vec = a - b.vec, bivec = b.bivec };

        public static implicit operator matrix2f(resizer2f a) => new matrix2f()
        {
            x = new float2(a.xx, a.xy),
            y = new float2(a.xy, a.yy),
        };

        public static explicit operator rotor2f(resizer2f a) => new rotor2f(math.sqrt(a.xx), 0);

        public float area => xx + yy - xy * xy;
    }

    /// <summary>
    /// Transformation resizing space along some 3 orthogonal directions. 
    /// Commonly known as a symmetric matrix.
    /// </summary>
    public struct resizer3f
    {
        public float3 vec;
        public float3 bivec;

        public float xx => vec.x;
        public float yy => vec.y;
        public float zz => vec.z;
        public float xy => bivec.z;
        public float yx => bivec.z;
        public float zx => bivec.y;
        public float xz => bivec.y;
        public float yz => bivec.x;
        public float zy => bivec.x;

        public float3 x => new float3(xx, xy, xz);
        public float3 y => new float3(yx, yy, yz);
        public float3 z => new float3(zx, zy, zz);

        #region Initializations
        public resizer3f(float3 size)
        {
            this.vec = size;
            this.bivec = float3.zero;
        }

        public resizer3f(float3 main, float3 off)
        {
            this.vec = main;
            this.bivec = off;
        }

        public static readonly resizer3f zero = default;
        public static readonly resizer3f identity = new resizer3f(float3.all(1), float3.zero);
        public static resizer3f fromScalar(float a) => new resizer3f(float3.all(a), float3.zero);
        public static resizer3f fromArrowResizer(float3 a, float scale)
        {
            float k = scale-1;
            return new resizer3f(
                new float3(a.x * a.x * k + 1, a.y * a.y * k + 1, a.z * a.z * k + 1),
                new float3(k * a.y * a.z, k * a.z * a.x, k * a.x * a.y)
            );
        }
        #endregion

        public override string ToString() => $"[{x};  {y};  {z}]";

        /// <summary>
        /// Transformation turning force into resized torque multiplied by arm length. In other words, results in force -> tangential velocity.
        /// </summary>
        public resizer3f outsideTorqueSpace(float3 arm)
        {
            /* Naive
            var a = this;
            var xzy = arm.x * a.off.yz;
            var yzx = arm.y * a.off.xz;
            var zyx = arm.z * a.off.xy;

            var iyy = zyx - xzy;
            var izz = xzy - yzx;

            var ixy = arm.y * a.off.yz - arm.z * a.main.y;
            var ixz = arm.y * a.main.z - arm.z * a.off.yz;
            var iyx = arm.z * a.main.x - arm.x * a.off.xz;

            var iyz = arm.z * a.off.xz - arm.x * a.main.z;
            var izx = arm.x * a.off.xy - arm.y * a.main.x;
            var izy = arm.x * a.main.y - arm.y * a.off.xy;

            vec3 vec = new(arm.y * ixz - arm.z * ixy, arm.z * iyx - arm.x * iyz, arm.x * izy - arm.y * izx);
            vec3 biv = new(arm.y * iyz - arm.z * iyy, arm.y * izz - arm.z * izy, arm.z * izx - arm.x * izz);
            */

            float q = arm.z, r = -arm.y, t = arm.x;
            float x = this.vec.x, y = this.vec.y, z = this.vec.z;
            float xy = bivec.z, zx = bivec.y, yz = bivec.x;

            float ct = yz*t;
            float br_ct = zx*r+ct, tz_bq = t*z - zx*q, ar_ty = xy*r + t*y, at_rx = xy*t + r*x;

            float3 vec = new float3(
                q*(yz*r + q*y) + r*(yz*q + r*z),
                q*(q*x - zx*t) + t*tz_bq,
                r*at_rx + t*ar_ty
            );

            float3 biv = new float3(
                 q*at_rx - t*br_ct,       //yz
                -q*ar_ty - r*br_ct,       //zx
                 q*(ct - xy*q) + r*tz_bq  //xy
            );

            return new resizer3f()
            {
                vec = vec,
                bivec = biv
            };
        }

        public resizer3f half
        {
            get
            {
                asSizeAndRotation(out float3 size, out matrix3f rotation);
                return math.sqrt(size).asResizerInsideRotation(rotation);
            }
        }

        //works flawlessly
        public resizer3f inverted
        {
            get
            {
                //21* 1/ 6+

                var xx_ = vec.y * vec.z - bivec.x * bivec.x;
                var xy_ = bivec.x * bivec.y - vec.z * bivec.z;
                var zx_ = bivec.z * bivec.x - bivec.y * vec.y;
                var invDet = 1f / (xx_ * vec.x + xy_ * bivec.z + zx_ * bivec.y);
                var yy_ = vec.x * vec.z - bivec.y * bivec.y;
                var yz_ = bivec.y * bivec.z - vec.x * bivec.x;
                var zz_ = vec.x * vec.y - bivec.z * bivec.z;
                return new resizer3f()
                {
                    vec = new(invDet * xx_, invDet * yy_, invDet * zz_),
                    bivec = new(invDet * xy_, invDet * zx_, invDet * yz_)
                };
                /*

                float x = main.x, y = main.y, z = main.z;
                float a = off.z, b = off.y, c = off.x;
                float a_ = off.z*off.z, b_ = off.y*off.y, c_ = off.x*off.x;

                var invDet = 1f/(x*y*z + 2*a*b*c - (x*c_ + y*b_ + z*a_));
                return new resizer() {
                    main = new(invDet*(y*z - c_), invDet*(x*z - b_), invDet*(x*y - a_)),
                    off = new(invDet*(b*c - a*z), invDet*(a*c - b*y), invDet*(a*b - c*x))
                */
            }
        }

        /// <summary>
        /// Results in one of multiple possible aligned resizers. Commonly is referred to as diagonalization.
        /// </summary>
        public float3 size
        {
            get
            {
                var p1 = bivec.lengthSq;
                if(p1 == 0) return vec;
                const float _1by3 = 1f/3, _1by6 = 1f/6, _piBy3 = math.pi / 3, _2piBy3 = 2 * math.pi / 3;

                var q = (xx+yy+zz) * _1by3;
                var p = math.sqrt(_1by6 * (2 * p1 + (vec - q).lengthSq));
                var r = 0.5f * ((this - q) * (1f / p)).volume;

                var phi = 0f;
                if(r <= -1) { phi = _piBy3; }
                else if(r < 1) { phi = math.acos(r) * _1by3; }

                var p2 = 2*p;
                var a = p2 * math.cos(phi);
                var b = p2 * math.cos(phi + _2piBy3);
                return q + new float3(a, b, -(a + b));
            }
        }

        // Input eigenvalue must be unique in input matrix. 
        private float3 eigennormalFromUniqueEigenvalue(float eigenvalue)
        {
            var m = this - eigenvalue;
            var x = (m.y).cross(m.z);
            var y = (m.z).cross(m.x);
            var z = (m.x).cross(m.y);

            var ret = (x.lengthSq > y.lengthSq)? x : y;
            ret = (ret.lengthSq > z.lengthSq) ? ret : z;
            return ret.normal;
        }

        /// <summary>
        /// Separates resizer into a size and rotation.
        /// Can be used to easily make Tissot's indicatrix. 
        /// </summary>
        public void asSizeAndRotation(out float3 size, out matrix3f rotation)
        {
            size = this.size;
            rotation = matrix3f.identity;

            if(size.x == size.y)
            {
                if(size.y == size.z) { return; }

                rotation.z = eigennormalFromUniqueEigenvalue(size.z);
                rotation.y = (rotation.z).orthoNormal;
                rotation.x = (rotation.z).cross(rotation.y);
            }
            else if(size.z == size.x)
            {
                rotation.y = eigennormalFromUniqueEigenvalue(size.y);
                rotation.x = (rotation.y).orthoNormal;
                rotation.z = (rotation.x).cross(rotation.y);
            }
            else if(size.y == size.z)
            {
                rotation.x = eigennormalFromUniqueEigenvalue(size.x);
                rotation.y = (rotation.x).orthoNormal;
                rotation.z = (rotation.y).cross(rotation.x);
            }
            else
            {
                rotation.x = eigennormalFromUniqueEigenvalue(size.x);
                rotation.y = eigennormalFromUniqueEigenvalue(size.y);
                rotation.z = (rotation.x).cross(rotation.y);
            }

            var v = volume;
            if(!float.IsNaN(v)) rotation *= math.sign(v);
        }

        public matrix3f then(resizer3f b)
        {
            return ((matrix3f) this).then(b);
        }

        public matrix3f then(rotor3f b)
        {
            return ((matrix3f) this).then(b);
        }

        public static resizer3f operator *(resizer3f a, float b) => new resizer3f { vec = a.vec * b, bivec = a.bivec * b };
        public static resizer3f operator *(float b, resizer3f a) => new resizer3f { vec = a.vec * b, bivec = a.bivec * b };
        public static resizer3f operator +(resizer3f a, resizer3f b) => new resizer3f { vec = a.vec + b.vec, bivec = a.bivec + b.bivec };
        public static resizer3f operator -(resizer3f a, resizer3f b) => new resizer3f { vec = a.vec - b.vec, bivec = a.bivec - b.bivec };
        public static resizer3f operator +(resizer3f a, float b) => new resizer3f { vec = a.vec + b, bivec = a.bivec };
        public static resizer3f operator -(resizer3f a, float b) => new resizer3f { vec = a.vec - b, bivec = a.bivec };
        public static resizer3f operator +(float a, resizer3f b) => new resizer3f { vec = b.vec + a, bivec = b.bivec };
        public static resizer3f operator -(float a, resizer3f b) => new resizer3f { vec = a - b.vec, bivec = b.bivec };

        public static implicit operator matrix3f(resizer3f a) => new matrix3f()
        {
            x = new float3(a.xx, a.xy, a.zx),
            y = new float3(a.xy, a.yy, a.yz),
            z = new float3(a.zx, a.zy, a.zz)
        };

        public static explicit operator rotor3f(resizer3f a) => new rotor3f(math.sqrt(a.xx), float3.zero);

        public float volume => xx * yy * zz + 2 * yx * zx * zy - (xx * zy * zy + yy * zx * zx + zz * yx * yx);
    }
}
