namespace WebApplication1.Models.Papiers
{
    public class DonneeArticle
    {
        public int ID { get; private set; }
        public int DonneeID { get; set; }
        public int ArticleID { get; set; }
        public int Quantite { get; set; }
    }
}