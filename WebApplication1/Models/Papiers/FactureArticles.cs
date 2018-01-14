namespace WebApplication1.Models.Papiers
{
    public class FactureArticle
    {
        public int ID { get; private set; }
        public int FactureID { get; set; }
        public int ArticleID { get; set; }
        public int Quantite { get; set; }
    }
}