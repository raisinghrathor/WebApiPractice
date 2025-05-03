namespace WebApiPractice.Models.Repositories
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>()
        {
            new Shirt{ ShirtId=1, Brand="My Brand", Color="Black", Gender="Male", Price=1000, Size=8},
            new Shirt{ ShirtId=2, Brand="My Brand", Color="Blue", Gender="female", Price=800, Size=7},
            new Shirt{ ShirtId=3, Brand="Your Brand", Color="White", Gender="Male", Price=90, Size=10},
            new Shirt{ ShirtId=4, Brand="Your Brand", Color="Yellow", Gender="female", Price=400, Size=9}

        };
        public static List<Shirt> GetShirts()
        {
            return shirts;
        }
        public static bool ShirtExists(int id)
        {
            return shirts.Any(x => x.ShirtId == id);

        }
        public static Shirt? GetShirtById(int shirtId)
        {
            return shirts.FirstOrDefault(s=>s.ShirtId==shirtId);
        }
        public static Shirt? GetShirtByProperties(string? brand,string? gender,string?color,int? size)
        {
            return shirts.FirstOrDefault(x=>!string.IsNullOrWhiteSpace(brand) &&
                                            !string.IsNullOrWhiteSpace(x.Brand)&&
                                    x.Brand.Equals(brand,StringComparison.OrdinalIgnoreCase)&&
                                   !string.IsNullOrWhiteSpace(gender) &&
                                            !string.IsNullOrWhiteSpace(x.Gender) &&
                                    x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase)&&
                                    !string.IsNullOrWhiteSpace(color) &&
                                            !string.IsNullOrWhiteSpace(x.Color) &&
                                    x.Color.Equals(color, StringComparison.OrdinalIgnoreCase)&&
                                    size.HasValue&&
                                    x.Size.HasValue&&
                                    size.Value==x.Size.Value);
        }
        public static void CreateShirt(Shirt shirt)
        {
            int MaxId = shirts.Max(x => x.ShirtId);
            shirt.ShirtId = MaxId + 1;
            shirts.Add(shirt);
        }
        public static void UpdateShirt(Shirt shirt)
        {
            var ExistingShirt = shirts.First(x=>x.ShirtId==shirt.ShirtId);
            ExistingShirt.Brand = shirt.Brand;
            ExistingShirt.Price = shirt.Price;
            ExistingShirt.Gender = shirt.Gender;
            ExistingShirt.Color = shirt.Color;

        }
        public static void DeleteShirt(int shirtId)
        {
            var shirt= ShirtRepository.GetShirtById(shirtId);
            if (shirt != null)
            {
                shirts.Remove(shirt);
            }
        }
    }
}
