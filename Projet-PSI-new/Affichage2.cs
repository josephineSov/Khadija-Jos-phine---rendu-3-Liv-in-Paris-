using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_PSI_new
{
    public class GraphViewer : Form
    {
        private Metros metros;

        public GraphViewer(Metros metros)
        {
            this.metros = metros;
            this.Text = "Affichage du Graphe Métro";
            this.Size = new Size(800, 600); 
            this.DoubleBuffered = true; 
            this.Paint += GraphViewer_Paint;
        }

        private void GraphViewer_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            
            var random = new Random();
            var positions = new Dictionary<Noeud, Point>();
            foreach (var station in metros.stations)
            {
                
                positions[station] = new Point(random.Next(50, 750), random.Next(50, 550));
            }

            
            foreach (var lien in metros.liens)
            {
                var source = lien.Source;
                var destination = lien.Destination;

                if (positions.ContainsKey(source) && positions.ContainsKey(destination))
                {
                    var srcPos = positions[source];
                    var destPos = positions[destination];

                    
                    g.DrawLine(Pens.Black, srcPos, destPos);
                }
            }

            
            foreach (var station in metros.stations)
            {
                if (positions.ContainsKey(station))
                {
                    var position = positions[station];
                    var rect = new Rectangle(position.X - 10, position.Y - 10, 20, 20);

                    
                    g.FillEllipse(Brushes.LightBlue, rect);
                    g.DrawEllipse(Pens.Blue, rect);

                    
                    g.DrawString(station.Nom, new Font("Arial", 8), Brushes.Black, position.X + 10, position.Y - 10);
                }
            }
        }
    }
}
