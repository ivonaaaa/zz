﻿using MarketplaceApp.Domain;
using MarketplaceApp.Domain.Dtos;
using System;
using System.Collections.Generic;
using MarketplaceApp.Helpers;

namespace MarketplaceApp.Presentation.AllMenus
{
    public class SellerMenu
    {
        private readonly Marketplace _marketplace;
        private readonly SellerDto _seller;

        public SellerMenu(Marketplace marketplace, SellerDto seller)
        {
            _marketplace = marketplace;
            _seller = seller;
        }

        public void ShowSellerMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"\n--- Pozdrav, {_seller.Name}! ---");
                Console.WriteLine("1 - Dodaj proizvod");
                Console.WriteLine("2 - Pregledaj svoje proizvode");
                Console.WriteLine("3 - Pregledaj ukupnu zaradu");
                Console.WriteLine("4 - Pregledaj prodane proizvode po kategoriji");
                Console.WriteLine("5 - Pregledaj zaradu u određenom vremenskom razdoblju");
                Console.WriteLine("6 - Povratak na glavni izbornik");
                Console.Write("\nOdaberite opciju: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        ViewYourProducts();
                        break;
                    case "3":
                        ViewTotalEarnings();
                        break;
                    case "4":
                        ViewSoldProductsByCategory();
                        break;
                    case "5":
                        ViewEarningsForTimePeriod();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nevažeća opcija. Pokušajte ponovo.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Unesite naziv proizvoda:");
            string name = Console.ReadLine();
            Console.WriteLine("Unesite opis proizvoda:");
            string description = Console.ReadLine();

            decimal price;
            while (true)
            {
                Console.WriteLine("Unesite cijenu proizvoda:");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                Console.WriteLine("Nevažeća cijena. Molimo unesite pozitivnu decimalnu vrijednost.");
            }

            string category = Helper.SelectCategory();
            if (category != null)
            {
                _marketplace.AddProduct(name, description, price, _seller, category);
            }
            else Console.WriteLine("Nevažeći odabir kategorije. Pokušajte ponovo.");
            Console.ReadKey();
        }


        private void ViewYourProducts()
        {
            Console.Clear();
            Console.WriteLine("Pregled proizvoda. Pritisnite bilo koju tipku za povratak na izbornik...");

            var products = _marketplace.ViewProducts(_seller);
            if (products.Count == 0)
                Console.WriteLine("Nemate proizvode.");
            else
            {
                foreach (var product in products)
                    Console.WriteLine($"Naziv: {product.Name}, Cijena: {product.Price}, Status: {product.Status}");
            }
            Console.ReadKey();
        }


        private void ViewTotalEarnings()
        {
            Console.Clear();
            var earnings = _marketplace.ViewTotalEarnings(_seller);
            Console.WriteLine($"Ukupna zarada: {earnings}");
            Console.ReadKey();
        }

        private void ViewSoldProductsByCategory()
        {
            Console.Clear();
            string category = Helper.SelectCategory();
            if (category != null)
            {
                var soldProducts = _marketplace.ViewSoldProductsByCategory(_seller, category);
                if (soldProducts.Count == 0)
                    Console.WriteLine("Nema prodanih proizvoda u toj kategoriji.");
                else
                {
                    foreach (var product in soldProducts)
                        Console.WriteLine($"Naziv: {product.Name}, Cijena: {product.Price}");
                }
            }
            else Console.WriteLine("Neispravan odabir kategorije.");
            Console.ReadKey();
        }


        private void ViewEarningsForTimePeriod()
        {
            Console.Clear();
            Console.WriteLine("Pregled zarade u vremenskom razdoblju...");

            DateTime startDate;
            DateTime endDate;
            while (true)
            {
                Console.WriteLine("Unesite datum početka (yyyy-MM-dd):");
                string startInput = Console.ReadLine();
                if (DateTime.TryParseExact(startInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDate))
                    break;
                else Console.WriteLine("Nevažeći format datuma. Molimo unesite datum u formatu 'yyyy-MM-dd'.");
            }

            while (true)
            {
                Console.WriteLine("Unesite datum završetka (yyyy-MM-dd):");
                string endInput = Console.ReadLine();
                if (DateTime.TryParseExact(endInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDate))
                {
                    if (endDate >= startDate)
                        break;
                    else Console.WriteLine("Završni datum mora biti jednak ili nakon početnog datuma. Pokušajte ponovo.");
                }
                else Console.WriteLine("Nevažeći format datuma. Molimo unesite datum u formatu 'yyyy-MM-dd'.");
            }
            Console.Clear();
            decimal earnings = _marketplace.ViewEarningsInTimePeriod(_seller, startDate, endDate);
            Console.WriteLine($"Zarada u odabranom vremenskom razdoblju: {earnings}");
            Console.WriteLine("\nPritisnite bilo koju tipku za povratak na izbornik...");
            Console.ReadKey();
        }
    }
}
