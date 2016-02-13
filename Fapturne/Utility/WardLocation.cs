using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using SharpDX;

namespace Fapturne
{
    internal class WardLocation
    {
        public List<Vector3> Normal = new List<Vector3>();
        public WardLocation()
        {
            CreateTables();

            var list = (from pos in Normal
                        let x = pos.X
                        let y = pos.Y
                        let z = pos.Z
                        select new Vector3(x, y, z)).ToList();
            Normal = list;
        }
        private void CreateTables()
        {
                if (FapturneMenu.checkWard())
                {
                    Normal.Add(new Vector3(9918f, 6538f, 33.13258f));
                    Normal.Add(new Vector3(12504f, 1490f, 53.74172f));
                    Normal.Add(new Vector3(13312f, 2448f, 51.3669f));
                    Normal.Add(new Vector3(11852f, 3940f, -68.11531f));
                    Normal.Add(new Vector3(8180f, 5194f, 52.64763f));
                    Normal.Add(new Vector3(6496f, 4676f, 48.5272f));
                    Normal.Add(new Vector3(3416f, 7706f, 52.13334f));
                    Normal.Add(new Vector3(2340f, 9692f, 54.17554f));
                    Normal.Add(new Vector3(4710f, 10000f, -71.23711f));
                    Normal.Add(new Vector3(6850f, 9662f, 54.40515f));
                    Normal.Add(new Vector3(6868f, 11398f, 53.82961f));
                    Normal.Add(new Vector3(8286f, 10182f, 50.06982f));
                    Normal.Add(new Vector3(3074f, 10784f, -70.27567f));
                    Normal.Add(new Vector3(4484f, 11804f, 56.8484f));
                    Normal.Add(new Vector3(2364f, 13474f, 52.8381f));
                    Normal.Add(new Vector3(1136f, 12322f, 52.8381f));
                    Normal.Add(new Vector3(4460f, 11794f, 56.8484f));
                    Normal.Add(new Vector3(9982f, 7730f, 51.75227f));
                    Normal.Add(new Vector3(11450f, 7212f, 51.7251f));
                    Normal.Add(new Vector3(12546f, 5192f, 51.7294f));
                    Normal.Add(new Vector3(7800f, 3566f, 52.53794f));
                }
            }
        }
    }
