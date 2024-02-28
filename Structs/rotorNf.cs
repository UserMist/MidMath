namespace MidMath
{

    /// <summary>
    /// Transformation applying rotation and non-negative uniform-resizing.
    /// Commonly known as a complex number. Usually is expected to be normalized, so it only represents a rotation.
    /// </summary>
    public struct rotor2f
    {
        public float s;
        public float bivec;

        #region Initializations
        public rotor2f(float s, float bivec)
        {
            this.s = s;
            this.bivec = bivec;
        }

        public static readonly rotor2f zero = default;
        public static readonly rotor2f identity = new rotor2f(1, 0);
        public static rotor2f fromScalar(float a) => new rotor2f(a, 0);
        #endregion

        public override string ToString() => ((matrix2f) this).ToString();

        private float scaleSq => s * s + bivec * bivec;
        public float scale => math.sqrt(scaleSq);

        public float area => scaleSq;

        public rotor2f normal
        {
            get
            {
                var m = 1f / math.sqrt(scaleSq);
                //m = s < 0 ? -m : m;
                var r = new rotor2f(m * s, m * bivec);
                return r;
            }
        }

        public rotor2f antirotary => new rotor2f(s, -bivec);
        public rotor2f inverted
        {
            get
            {
                var m = 1f / scaleSq;
                return new rotor2f(m * s, -m * bivec);
            }
        }

        public rotor2f then(rotor2f b)
        {
            return new rotor2f(s * b.s - bivec * b.bivec, bivec * b.s + s * b.bivec);
        }

        public rotor2f half { get { var ret = normal; ret.s += 1; return ret.normal; } }

        public rotor2f rawAddHalf(rotor2f b) => new rotor2f(s + b.s, bivec + b.bivec);
        public rotor2f rawSubHalf(rotor2f b) => new rotor2f(s - b.s, bivec - b.bivec);

        public float angle => math.acos(s); //assumes rotor is normal

        /// <summary>
        /// Angular vector/bivector. Doesn't care if rotor is normal.
        /// </summary>
        public float orientedAngle => math.atan2(bivec, s);

        public rotor2f withOrientedAngle(float a)
        {
            return new rotor2f
            {
                s = math.cos(a),
                bivec = math.sin(a)
            };
        }

        public float safeAngle => math.acos(s/scale) * 2;

        public static implicit operator matrix2f(rotor2f a)
        {
            return new matrix2f(float2.unitX.transformedTo(a), float2.unitY.transformedTo(a));
        }

        public static explicit operator resizer2f(rotor2f a) => new resizer2f(float2.all(a.s));
    }

    /// <summary>
    /// Transformation applying rotation and non-negative uniform-resizing.
    /// Commonly known as a quaternion. Usually is expected to be normalized, so it only represents a rotation.
    /// </summary>
    public struct rotor3f
    {
        public float s;
        public float3 bivec;

        public float _xy => bivec.z;
        public float _yx => -bivec.z;
        public float _zx => bivec.y;
        public float _xz => -bivec.y;
        public float _yz => bivec.x;
        public float _zy => -bivec.x;

        #region Initializations
        public rotor3f(float s, float3 bivec)
        {
            this.s = s;
            this.bivec = bivec;
        }

        public static readonly rotor3f zero = default;
        public static readonly rotor3f identity = new rotor3f(1, float3.zero);
        public static rotor3f fromScalar(float a) => new rotor3f(math.sqrt(a), float3.zero);
        #endregion

        public override string ToString() => ((matrix3f) this).ToString();

        private float scalePow4 => s * s + bivec.lengthSq;
        private float scaleSq => math.sqrt(scalePow4);
        public float scale => math.sqrt(scaleSq);

        public float volume => scalePow4 / scale;

        public rotor3f normal
        {
            get
            {
                var m = 1f / math.sqrt(scalePow4);
                //m = s < 0 ? -m : m;
                var r = new rotor3f(m * s, m * bivec);
                return r;
            }
        }

        public rotor3f antirotary => new rotor3f(s, -bivec);
        public rotor3f inverted
        {
            get
            {
                var m = 1f / scalePow4;
                return new rotor3f(m * s, -m * bivec);
            }
        }

        public rotor3f then(rotor3f b)
        {
            return new rotor3f(s * b.s - bivec.dot(b.bivec), bivec * b.s + s * b.bivec + bivec.biwedge(b.bivec));
        }

        public rotor3f half { get { var ret = normal; ret.s += 1; return ret.normal; } }

        public rotor3f rawAdd(rotor3f b) => new rotor3f(s + b.s, bivec + b.bivec);
        public rotor3f rawSub(rotor3f b) => new rotor3f(s - b.s, bivec - b.bivec);

        public float angle => math.acos(s) * 2; //assumes rotor is normal

        /// <summary>
        /// Angular vector/bivector
        /// </summary>
        public float3 orientedAngle
        {
            get //assumes rotor is normal
            {
                return bivec.safeNormal * angle;
            }
            set
            {
                var l = value.length;
                bivec = value.safeNormal * math.sin(0.5f * l);
                s = math.cos(0.5f * l);
            }
        }

        public rotor3f withOrientedAngle(float3 angular)
        {
            var l = angular.length;
            return new rotor3f
            {
                s = math.cos(0.5f * l),
                bivec = angular.safeNormal * math.sin(0.5f * l)
            };
        }

        public rotor3f withAngle(float l)
        {
            return new rotor3f
            {
                s = math.cos(0.5f * l),
                bivec = bivec.safeNormal * math.sin(0.5f * l)
            };
        }

        public float safeAngle => math.acos(s/scale) * 2;
        public float3 safeOrientedAngle => bivec * (1f / normal.safeAngle);

        public static implicit operator matrix3f(rotor3f a)
        {
            return new matrix3f(float3.unitX.transformedTo(a), float3.unitY.transformedTo(a), float3.unitZ.transformedTo(a));
        }

        public static explicit operator resizer3f(rotor3f a) => new resizer3f(float3.all(a.s * a.s));
    }
}
