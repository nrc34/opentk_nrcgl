using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class Tween
    {
        /// <summary>
        /// Tween function.
        /// </summary>
        public enum Function
        {
            Linear,
            Back,
            Cubic,
            Circular,
            Bounce,
            Exponential,
            Elastic,
            Quadratic,
            Quartic,
            Quintic,
            Sinusoidal
        }

        /// <summary>
        /// Tween Ease type.
        /// </summary>
        public enum Ease
        {
            None,
            In,
            Out,
            InOut
        }

        /// <summary>
        /// Solve the tween, from a to b at the given tick.
        /// </summary>
        /// <param name="function">Tween function.</param>
        /// <param name="ease">Ease type.</param>
        /// <param name="a">Initial value</param>
        /// <param name="b">Final value</param>
        /// <param name="totalTicks">Total ticks for the tween between a and b.</param>
        /// <param name="tick">Tick to be solved.</param>
        /// <returns></returns>
        public static float Solve(Function function, Ease ease, float a, float b, float totalTicks, float tick)
        {

            switch (function)
            {
                #region Linear
                case Function.Linear:
                    switch (ease)
	                {
                        case Ease.None:
                            return (b * tick) / (totalTicks + a);
                        case Ease.In:
                            return (b * tick) / (totalTicks + a);
                        case Ease.Out:
                            return (b * tick) / (totalTicks + a);
                        case Ease.InOut:
                            return (b * tick) / (totalTicks + a);
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Back
                case Function.Back:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return b * (tick /= totalTicks) * tick * 
                                ((1.70158f + 1) * tick - 1.70158f) + a;
                        case Ease.Out:
                            return b * ((tick = tick / totalTicks - 1) * 
                                tick * ((1.70158f + 1) * tick + 1.70158f)
                                + 1) + a;
                        case Ease.InOut:
                            float s = 1.70158f;
                            if ( ( tick /= totalTicks / 2 ) < 1 )
                                return b / 2 * ( tick * tick * ( ( ( s *= 
                                    ( 1.525f ) ) + 1 ) * tick - s ) ) + a;
                            return b / 2 * ( ( tick -= 2 ) * tick * ( ( ( s *= 
                                ( 1.525f ) ) + 1 ) * tick + s ) + 2 ) + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Cubic
                case Function.Cubic:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return b * (tick /= totalTicks) 
                                * tick * tick + a;
                        case Ease.Out:
                            return b * ((float)Math.
                                Pow((tick / totalTicks) - 1, 3) + 1) + a;
                        case Ease.InOut:
                            if ( (tick /= totalTicks / 2) < 1)
                                return (b / 2) * tick * tick * tick + a;
                            else
                                return (b / 2) *  (( tick -= 2) * tick * tick + 2) + a;;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Circular
                case Function.Circular:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return -b * ((float)Math.Sqrt(1 - 
                                (tick /= totalTicks) * tick) - 1) + a;
                        case Ease.Out:
                            return b * (float)Math.Sqrt(1 - (tick = tick 
                                / totalTicks - 1) * tick) + a;
                        case Ease.InOut:
                            if ( ( tick /= totalTicks / 2 ) < 1 )
                            return -b / 2 * ( (float)Math.Sqrt( 1 - tick * 
                                tick ) - 1 ) + a;

                            return b / 2 * ( (float)Math.Sqrt( 1 - ( tick -= 
                                2 ) * tick ) + 1 ) + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Bounce
                case Function.Bounce:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            break;
                        case Ease.Out:
                            if ((tick /= totalTicks) < (1 / 2.75))
                                return b * (7.5625f * tick * tick) + a;
                            else if (tick < (2 / 2.75))
                                return b * (7.5625f * (tick -= (1.5f / 2.75f)) * tick + 0.75f) + a;
                            else if (tick < (2.5f / 2.75f))
                                return b * (7.5625f * (tick -= (2.25f / 2.75f)) * tick + .9375f) + a;
                            else
                                return b * (7.5625f * (tick -= (2.625f / 2.75f)) * tick + 0.984375f) + a;
                        case Ease.InOut:
                            break;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Exponecial
                case Function.Exponential:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return (tick == 0) ? 
                                a : 
                                b * (float)Math.
                                Pow(2, 10 * (tick / totalTicks - 1)) + a;
                        case Ease.Out:
                            return (tick == totalTicks) ? 
                                a + b : 
                                b * ((float)-Math.
                                Pow(2, -10 * tick / totalTicks) + 1) + a;
                        case Ease.InOut:
                            if ( tick == 0 )
                            return a;

                            if ( tick == totalTicks )
                            return a + b;

                            if ( ( tick /= totalTicks / 2 ) < 1 )
                            return b / 2 * (float)Math.Pow( 2, 10 * ( tick - 1 ) ) + a;

                            return b / 2 * ((float)-Math.Pow( 2, -10 * --tick ) + 2 ) + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Elastic
                case Function.Elastic:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            if ( ( tick /= totalTicks ) == 1 )
                            return a + b;

                            float p = totalTicks * 0.3f;
                            float s = p / 4;

                            return -( b * (float)Math.Pow( 2, 10 * 
                                ( tick -= 1 ) ) * (float)Math.Sin( 
                                ( tick * totalTicks - s ) * ( 2 * 
                                (float)Math.PI ) / p ) ) + a;
                        case Ease.Out:
                            if ( ( tick /= totalTicks ) == 1 )
                            return a + b;

                            float p1 = totalTicks * 0.3f;
                            float s1 = p1 / 4;

                            return ( b * (float)Math.Pow( 2, -10 * tick ) * 
                                (float)Math.Sin( ( tick * totalTicks - s1 ) * 
                                ( 2 * (float)Math.PI ) / p1 ) + b + a );
                        case Ease.InOut:
                            if ( ( tick /= totalTicks / 2 ) == 2 )
                            return a + b;

                            float p2 = totalTicks * ( 0.3f * 1.5f );
                            float s2 = p2 / 4;

                            if ( tick < 1 )
                            return -0.5f * ( b * (float)Math.Pow( 2, 10 * 
                                ( tick -= 1 ) ) * (float)Math.Sin( ( tick * 
                                totalTicks - s2 ) * ( 2 * (float)Math.PI ) /
                                p2 ) ) + a;
                            return b * (float)Math.Pow( 2, -10 * 
                                ( tick -= 1 ) ) * (float)Math.Sin( ( tick *
                                totalTicks - s2 ) * ( 2 * (float)Math.PI ) /
                                p2 ) * 0.5f + b + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Quadratic
                case Function.Quadratic:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return b * (tick /= totalTicks) * 
                                tick + a;
                        case Ease.Out:
                            return -b * (tick /= totalTicks) * 
                                (tick - 2) + a;
                        case Ease.InOut:
                            if ( ( tick /= totalTicks / 2 ) < 1 )
                             return b / 2 * tick * tick + a;

                             return -b / 2 * ( ( --tick ) * 
                                 ( tick - 2 ) - 1 ) + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Quartic
                case Function.Quartic:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            break;
                        case Ease.Out:
                            break;
                        case Ease.InOut:
                            break;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Quintic
                case Function.Quintic:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            return b * (tick /= totalTicks) * 
                                tick * tick * tick * tick + a;
                        case Ease.Out:
                            return b * ((tick = tick / totalTicks - 1) 
                                * tick * tick * tick * tick + 1) + a;
                        case Ease.InOut:
                            if ( ( tick /= totalTicks / 2 ) < 1 )
                            return b / 2 * tick * tick * 
                                tick * tick * tick + a;
                            return b / 2 * ( ( tick -= 2 ) * 
                                tick * tick * tick * tick + 2 ) + a;
                        default:
                            break;
	                }
                    break;
                #endregion
                #region Sinusoidal
                case Function.Sinusoidal:
                    switch (ease)
	                {
                        case Ease.None:
                            break;
                        case Ease.In:
                            break;
                        case Ease.Out:
                            break;
                        case Ease.InOut:
                            break;
                        default:
                            break;
	                }
                    break;
                #endregion
                default:
                    break;
            }

            return a;
        }
    }
}
