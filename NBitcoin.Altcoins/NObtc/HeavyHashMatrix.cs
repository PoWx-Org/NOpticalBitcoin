using System;

namespace NBitcoin.Altcoins 
{
    public class HeavyHashMatrix {
        public static double RoundOffError = 1e-8;
        public static int RANK = 64;
        private ulong[,] _body = new ulong[RANK, RANK];

        public ulong[,] Body 
        { 
            get {
                return _body;
            }
        }
        public HeavyHashMatrix(uint256 seed) => PopulateMatrixBody(seed);
        public HeavyHashMatrix(ulong[,] body) => _body = body;

        private void PopulateMatrixBody(uint256 seed) {
            XoShiRo256PlusPlus prng = new XoShiRo256PlusPlus(seed);
            for (int i = 0; i < RANK; i++)
            {
                for (int j = 0; j < RANK; j += 16)
                {
                    ulong value = prng.GetNext();
                    for (int shift = 0; shift < 16; shift++)
                    {
                        _body[i, j + shift] = (value >> (4 * shift)) & 0xF;
                    }
                }
            }
        }

        // Uses LU decomposition with consequent diagonal entrance checks
        private bool IsFullRank()
        {
            // LU	
            double[,] lu = new double[RANK, RANK];
            double sum = 0;
            for (int i = 0; i < RANK; i++)
            {
                for (int j = i; j < RANK; j++)
                {
                    sum = 0;
                    for (int k = 0; k < i; k++)
                        sum += lu[i, k] * lu[k, j];
                    lu[i, j] = _body[i, j] - sum;
                }
                for (int j = i + 1; j < RANK; j++)
                {
                    sum = 0;
                    for (int k = 0; k < i; k++)
                        sum += lu[j, k] * lu[k, i];
                    if (lu[i, i] == 0)
                    {
                        lu[j, i] = 0;
                    } else 
                    {
                        lu[j, i] = (1 / lu[i, i]) * (_body[j, i] - sum);
                    }

                }
            }

            // Checks diagonal entrances to be non-zero
            for (int i = 0; i < RANK; i++)
            {
                if(Math.Abs(lu[i, i]) < RoundOffError) {
                    return false;
                }
            }
            return true;
        }

        public override string ToString() {
            string _str = "{";
            for (int i = 0; i < RANK; i++)
            {
                for (int j = 0; j < RANK; j++)
                {
                    if (j == 0) 
                    {
                        _str += "{ " + _body[i, j] + ", ";
                    } else if(j == RANK - 1) 
                    {
                        _str += _body[i, j] + " }";
                    } else 
                    {
                        _str += _body[i, j] + ", ";
                    }
                    
                }
                _str += " }";
            }
            return _str;
        }
    }
}