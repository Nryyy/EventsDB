using CsvHelper;
using Managment.Classes;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO; // Додано для StreamReader
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Helpers
{
    public class CategoryParser
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryParser(CategoryRepository categoryRepository) // Виправлено ім'я конструктора
        {
            _categoryRepository = categoryRepository;
        }

        public void ParseAndStoreCategories(string csvFilePath)
        {
            var categories = new List<Category>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var categoryName = csv.GetField<string>("Categories");
                    var category = new Category { Name = categoryName };
                    categories.Add(category);
                }
            }

            StoreCategoriesInDatabase(categories);
        }

        private void StoreCategoriesInDatabase(List<Category> categories)
        {
            foreach (var category in categories)
            {
                _categoryRepository.Create(category);
            }
            _categoryRepository.SaveChanges();
        }
    }
}
