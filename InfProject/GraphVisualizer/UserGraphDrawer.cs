using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer
{
    public class UserGraphDrawer: GraphDrawer, IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawGraph(DataFromUser.Graph, canvas);
        }
    }
}
