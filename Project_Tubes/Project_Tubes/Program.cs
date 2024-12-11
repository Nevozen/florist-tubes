using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("||                 Florist                   ||");
            Console.WriteLine("==============================================");
            Console.WriteLine("Pilih User: ");
            Console.WriteLine("1. Pelanggan");
            Console.WriteLine("2. Admin");
            Console.WriteLine("0. Exit");
            Console.Write("Pilihan: ");
            int userChoice;
            if (!int.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("Input tidak valid. Masukkan angka.");
                Console.ReadKey();
                continue;
            }

            if (userChoice == 1)
            {
                PelangganMenu();
            }
            else if (userChoice == 2)
            {
                AdminMenu();
            }
            else if (userChoice == 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadKey();
            }
        }
    }

    private static void AdminMenu()
    {
        string connectionString = "Server=localhost;Database=florist;User ID=root;Password=;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Login admin
            int maxAttempts = 3;
            bool isAuthenticated = false;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("||              Login Admin                ||");
                Console.WriteLine("==============================================");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                // Cek ke database
                using (var command = new MySqlCommand("SELECT COUNT(*) FROM users WHERE Username = @username AND Password = @password", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int result = Convert.ToInt32(command.ExecuteScalar());
                    if (result > 0)
                    {
                        isAuthenticated = true;
                        Console.WriteLine("\nWelcome, Admin!");
                        Console.WriteLine("Tekan enter untuk melanjutkan..");
                        Console.ReadKey();
                        break;
                    }
                }

                // Jika login gagal
                Console.WriteLine($"\nUsername/Password Salah! Percobaan tersisa {maxAttempts - attempt} kali lagi.");
                if (attempt < maxAttempts)
                {
                    Console.WriteLine("Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nCoba lagi dalam 5 menit.");
                    Console.ReadKey();
                    return; // Kembali ke menu utama
                }
            }

            // Jika login berhasil, masuk ke menu admin
            if (isAuthenticated)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("==============================================");
                    Console.WriteLine("||              Admin Menu                 ||");
                    Console.WriteLine("==============================================");
                    Console.WriteLine("1. Lihat Semua Stok");
                    Console.WriteLine("2. Tambah Bunga");
                    Console.WriteLine("3. Update Harga Bunga");
                    Console.WriteLine("4. Hapus Bunga");
                    Console.WriteLine("5. Cari Bunga Berdasarkan ID");
                    Console.WriteLine("6. Filter Berdasarkan Harga");
                    Console.WriteLine("0. Kembali");
                    Console.WriteLine("==============================================");
                    Console.Write("Pilihan: ");
                    int choice = int.Parse(Console.ReadLine());

                    if (choice == 0) break; // Kembali ke menu sebelumnya

                    // Pilihan menu admin
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("\n========== Daftar Stok ==========");
                            using (var command = new MySqlCommand("SELECT * FROM Flowers", connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("\nTekan Enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 2:
                            Console.Write("\nMasukkan Nama Bunga: ");
                            string newName = Console.ReadLine();
                            Console.Write("Masukkan Harga Bunga: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command = new MySqlCommand("INSERT INTO Flowers (Name, Price) VALUES (@name, @price)", connection))
                            {
                                command.Parameters.AddWithValue("@name", newName);
                                command.Parameters.AddWithValue("@price", newPrice);
                                command.ExecuteNonQuery();
                            }
                            Console.WriteLine("\nBunga berhasil ditambahkan!");
                            Console.WriteLine("\nTekan Enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 3:
                            Console.WriteLine("\n========== Update Harga Bunga ==========");
                            DisplayAllFlowers(connection);
                            Console.Write("\nMasukkan ID Bunga yang ingin diupdate: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            Console.Write("Masukkan Harga Baru: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal updatedPrice))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command = new MySqlCommand("UPDATE Flowers SET Price = @price WHERE Id = @id", connection))
                            {
                                command.Parameters.AddWithValue("@price", updatedPrice);
                                command.Parameters.AddWithValue("@id", updateId);
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    Console.WriteLine("\nHarga berhasil diperbarui!");
                                    Console.WriteLine("Tekan Apapun untuk Kembali..");
                                }
                                else
                                {
                                    Console.WriteLine("\nID tidak ditemukan.");
                                    Console.WriteLine("Tekan Apapun untuk Kembali..");
                                }
                                Console.ReadKey();

                            }

                            Console.ReadKey();
                            break;

                        case 4:
                            Console.WriteLine("\n========== Hapus Bunga ==========");
                            DisplayAllFlowers(connection);
                            Console.Write("\nMasukkan ID Bunga yang ingin dihapus: ");
                            if (!int.TryParse(Console.ReadLine(), out int deleteId))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command = new MySqlCommand("DELETE FROM Flowers WHERE Id = @id", connection))
                            {
                                command.Parameters.AddWithValue("@id", deleteId);
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    Console.WriteLine("Bunga berhasil dihapus!");
                                }
                                else
                                {
                                    Console.WriteLine("ID tidak ditemukan.");
                                }
                            }
                            Console.WriteLine("Tekan Apapun untuk Kembali..");

                            Console.ReadKey();
                            break;

                        case 5:
                            Console.WriteLine("\n========== Cari Bunga Berdasarkan ID ==========");
                            Console.Write("\nMasukkan ID Bunga: ");
                            if (!int.TryParse(Console.ReadLine(), out int searchId))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command = new MySqlCommand("SELECT * FROM Flowers WHERE Id = @id", connection))
                            {
                                command.Parameters.AddWithValue("@id", searchId);
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID tidak ditemukan.");
                                    }
                                }
                            }
                            Console.WriteLine("Tekan Apapun untuk Kembali..");

                            Console.ReadKey();
                            break;

                        case 6:
                            Console.WriteLine("\n========== Filter Berdasarkan Harga ==========");
                            Console.Write("\nMasukkan harga maksimum: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command = new MySqlCommand("SELECT * FROM Flowers WHERE Price <= @maxPrice", connection))
                            {
                                command.Parameters.AddWithValue("@maxPrice", maxPrice);
                                using (var reader = command.ExecuteReader())
                                {
                                    Console.WriteLine("\nHasil Filter:");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("Tekan Apapun untuk Kembali..");

                            Console.ReadKey();
                            break;

                        case 0:
                            Console.WriteLine("\nKembali ke menu sebelumnya..");
                            // Logika untuk keluar dari menu admin
                            return; // Keluar dari fungsi/method yang memuat switch ini
                            break;

                        default:
                            Console.WriteLine("\nPilihan tidak valid. Silakan coba lagi.");
                            Console.WriteLine("\nTekan Enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;
                    }

                }
            }
        }
    }


    private static void DisplayAllFlowers(MySqlConnection connection)
    {
        Console.WriteLine("\n========== Daftar Stok ==========");
        using (var command = new MySqlCommand("SELECT * FROM Flowers", connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                }
            }
        }
        
    }

    private static void AddFlower(MySqlConnection connection)
    {
        Console.Write("\nMasukkan Nama Bunga: ");
        string newName = Console.ReadLine();
        Console.Write("Masukkan Harga Bunga: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
        {
            Console.WriteLine("Harga tidak valid.");
            Console.ReadKey();
            return;
        }

        using (var command = new MySqlCommand("INSERT INTO Flowers (Name, Price) VALUES (@name, @price)", connection))
        {
            command.Parameters.AddWithValue("@name", newName);
            command.Parameters.AddWithValue("@price", newPrice);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Bunga berhasil ditambahkan!");
        Console.ReadKey();
    }

    private static void UpdateFlowerPrice(MySqlConnection connection)
    {
        DisplayAllFlowers(connection);

        Console.Write("\nMasukkan ID Bunga yang ingin diupdate: ");
        if (!int.TryParse(Console.ReadLine(), out int updateId))
        {
            Console.WriteLine("ID tidak valid.");
            Console.ReadKey();
            return;
        }

        Console.Write("Masukkan Harga Baru: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal updatedPrice))
        {
            Console.WriteLine("Harga tidak valid.");
            Console.ReadKey();
            return;
        }

        using (var command = new MySqlCommand("UPDATE Flowers SET Price = @price WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@price", updatedPrice);
            command.Parameters.AddWithValue("@id", updateId);
            if (command.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Harga berhasil diperbarui!");
            }
            else
            {
                Console.WriteLine("ID tidak ditemukan.");
            }
        }
        Console.ReadKey();
    }

    private static void DeleteFlower(MySqlConnection connection)
    {
        DisplayAllFlowers(connection);

        Console.Write("\nMasukkan ID Bunga yang ingin dihapus: ");
        if (!int.TryParse(Console.ReadLine(), out int deleteId))
        {
            Console.WriteLine("ID tidak valid.");
            Console.ReadKey();
            return;
        }

        using (var command = new MySqlCommand("DELETE FROM Flowers WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@id", deleteId);
            if (command.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Bunga berhasil dihapus!");
            }
            else
            {
                Console.WriteLine("ID tidak ditemukan.");
            }
        }
        Console.ReadKey();
    }

    private static void SearchFlowerById(MySqlConnection connection)
    {
        Console.Write("\nMasukkan ID Bunga: ");
        if (!int.TryParse(Console.ReadLine(), out int searchId))
        {
            Console.WriteLine("ID tidak valid.");
            Console.ReadKey();
            return;
        }

        using (var command = new MySqlCommand("SELECT * FROM Flowers WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@id", searchId);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                }
                else
                {
                    Console.WriteLine("ID tidak ditemukan.");
                }
            }
        }
        Console.ReadKey();
    }

    private static void FilterFlowersByPrice(MySqlConnection connection)
    {
        Console.Write("\nMasukkan harga maksimum: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
        {
            Console.WriteLine("Harga tidak valid.");
            Console.ReadKey();
            return;
        }

        using (var command = new MySqlCommand("SELECT * FROM Flowers WHERE Price <= @maxPrice", connection))
        {
            command.Parameters.AddWithValue("@maxPrice", maxPrice);
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("\nHasil Filter:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                }
            }
        }
        Console.ReadKey();
    }

    private static void PelangganMenu()
    {
        string customerName = "";
        string customerAddress = "";
        List<(int FlowerId, string FlowerName, int Quantity, decimal Subtotal)> cart = new List<(int, string, int, decimal)>();

        string connectionString = "Server=localhost;Database=florist;User ID=root;Password=;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Daftar Stok ==========");
                using (var command = new MySqlCommand("SELECT * FROM Flowers", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Nama: {reader["Name"]}, Harga: Rp {reader["Price"]}");
                        }
                    }
                }

                Console.WriteLine("\n========== Tambah Pesanan ==========");
                Console.Write("Masukkan ID Bunga yang ingin dibeli: ");
                if (!int.TryParse(Console.ReadLine(), out int flowerId))
                {
                    Console.WriteLine("ID tidak valid. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                // Check if the flower exists in the database
                string flowerName = null;
                decimal flowerPrice = 0;
                using (var command = new MySqlCommand("SELECT Name, Price FROM Flowers WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", flowerId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            flowerName = reader["Name"].ToString();
                            flowerPrice = Convert.ToDecimal(reader["Price"]);
                        }
                    }
                }

                if (flowerName == null)
                {
                    Console.WriteLine("ID bunga tidak ditemukan. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Masukkan jumlah: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Jumlah tidak valid. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                decimal subtotal = flowerPrice * quantity;
                cart.Add((flowerId, flowerName, quantity, subtotal));
                Console.WriteLine($"Bunga {flowerName} sebanyak {quantity} berhasil ditambahkan ke keranjang.");

                Console.WriteLine("\n========== Keranjang Belanja ==========");
                foreach (var item in cart)
                {
                    Console.WriteLine($"- {item.FlowerName}, Jumlah: {item.Quantity}, Subtotal: Rp {item.Subtotal}");
                }

                Console.Write("\nIngin menambahkan bunga lain? (y/n): ");
                string addMore = Console.ReadLine()?.ToLower();
                if (addMore != "y")
                {
                    break;
                }
            }

            // Konfirmasi Keranjang
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Konfirmasi Keranjang ==========");
                decimal totalPrice = 0;

                foreach (var item in cart)
                {
                    Console.WriteLine($"- {item.FlowerName}, Jumlah: {item.Quantity}, Subtotal: Rp {item.Subtotal}");
                    totalPrice += item.Subtotal;
                }

                Console.WriteLine($"Total Harga: Rp {totalPrice}");
                Console.Write("\nApakah keranjang sudah sesuai? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "y")
                {
                    break; // Lanjut ke detail pemesan
                }
                else if (confirmation == "n")
                {
                    Console.WriteLine("Silakan perbaiki keranjang.");
                    Console.ReadKey();
                    return; // Kembali ke menu pembelian
                }
                else
                {
                    Console.WriteLine("Input tidak valid. Harap masukkan 'y' atau 'n'.");
                    Console.ReadKey();
                }
            }

            // Input Detail Pemesan
            Console.Clear();
            Console.WriteLine("========== Detail Pemesan ==========");
            Console.Write("Masukkan Nama Anda: ");
            customerName = Console.ReadLine();
            Console.Write("Masukkan Alamat Anda: ");
            customerAddress = Console.ReadLine();

            // Simpan Pesanan ke Database
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Insert Order
                    decimal finalTotalPrice = cart.Sum(item => item.Subtotal);
                    long orderId;

                    using (var command = new MySqlCommand("INSERT INTO Orders (CustomerName, CustomerAddress, TotalPrice) VALUES (@name, @address, @totalPrice)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@name", customerName);
                        command.Parameters.AddWithValue("@address", customerAddress);
                        command.Parameters.AddWithValue("@totalPrice", finalTotalPrice);
                        command.ExecuteNonQuery();
                    }

                    // Get the last inserted OrderId
                    using (var command = new MySqlCommand("SELECT LAST_INSERT_ID()", connection, transaction))
                    {
                        orderId = Convert.ToInt64(command.ExecuteScalar());
                    }

                    // Insert Order Details
                    foreach (var item in cart)
                    {
                        using (var command = new MySqlCommand("INSERT INTO OrderDetails (OrderId, FlowerId, Quantity, Subtotal) VALUES (@orderId, @flowerId, @quantity, @subtotal)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", orderId);
                            command.Parameters.AddWithValue("@flowerId", item.FlowerId);
                            command.Parameters.AddWithValue("@quantity", item.Quantity);
                            command.Parameters.AddWithValue("@subtotal", item.Subtotal);
                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    Console.WriteLine("\nPesanan berhasil dibuat! Terima kasih telah berbelanja.");
                    Console.WriteLine();
                    Console.WriteLine("Tekan Apapun untuk Kembali..");
                    Console.ReadKey(); 

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
                }
            }
            Console.ReadKey();
        }
    }

}
