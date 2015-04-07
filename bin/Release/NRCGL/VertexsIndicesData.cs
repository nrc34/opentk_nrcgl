using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    public class VertexsIndicesData
    {
        
        public VertexFormat VertexFormat { get; set; }

        public int VertexCount { get; set; }

        public int IndicesCount { get; set; }

        public List<Vertex> Vertexs { get; set; }

        public List<uint> Indices { get; set; }
    }
}
