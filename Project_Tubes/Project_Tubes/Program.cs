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
            int PilihMenu_Kelompok1_SI4804;
            if (!int.TryParse(Console.ReadLine(), out PilihMenu_Kelompok1_SI4804)) // Jika pilihan bukan sama dengan angka
            {
                Console.WriteLine("Input tidak valid. Masukkan angka.");
                Console.ReadKey();
                continue;
            }
            if (PilihMenu_Kelompok1_SI4804 == 1)
            {
                PelangganMenu_Kelompok1_SI4804();
            }
            else if (PilihMenu_Kelompok1_SI4804 == 2)
            {
                AdminMenu_Kelompok1_SI4804();
            }
            else if (PilihMenu_Kelompok1_SI4804 == 0)
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

    private static void AdminMenu_Kelompok1_SI4804()
    {
        string connectionString_Kelompok1_SI4804 = "Server=localhost;Database=florist;UserID=root;Password=;";
        using (var connection_Kelompok1_SI4804 = new MySqlConnection(connectionString_Kelompok1_SI4804))
        {
            connection_Kelompok1_SI4804.Open();

            // Login admin
            int maxAttempts_Kelompok1_SI4804 = 3;
            bool isAuthenticated_Kelompok1_SI4804 = false;

            for (int attempt_Kelompok1_SI4804 = 1; attempt_Kelompok1_SI4804 <= maxAttempts_Kelompok1_SI4804; attempt_Kelompok1_SI4804++)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("||              Login Admin                ||");
                Console.WriteLine("==============================================");
                Console.Write("Username: ");
                string username_Kelompok1_SI4804 = Console.ReadLine();
                Console.Write("Password: ");
                string password_Kelompok1_SI4804 = Console.ReadLine();

                // Cek ke database
                using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT COUNT(*) FROM users WHERE Username = @username AND Password = @password", connection_Kelompok1_SI4804))
                {
                    command_Kelompok1_SI4804.Parameters.AddWithValue("@username", username_Kelompok1_SI4804);
                    command_Kelompok1_SI4804.Parameters.AddWithValue("@password", password_Kelompok1_SI4804);

                    int result_Kelompok1_SI4804 = Convert.ToInt32(command_Kelompok1_SI4804.ExecuteScalar());
                    if (result_Kelompok1_SI4804 > 0)
                    {
                        isAuthenticated_Kelompok1_SI4804 = true;
                        Console.WriteLine("\nWelcome, Admin!");
                        Console.WriteLine("Tekan enter untuk melanjutkan..");
                        Console.ReadKey();
                        break;
                    }
                }

                // Jika login gagal
                Console.WriteLine($"\nUsername/Password Salah! Percobaan tersisa {maxAttempts_Kelompok1_SI4804 - attempt_Kelompok1_SI4804} kali lagi.");
                if (attempt_Kelompok1_SI4804 < maxAttempts_Kelompok1_SI4804)
                {
                    Console.WriteLine("Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nCoba lagi nanti...");
                    Console.ReadKey();
                    return; // Kembali ke menu utama
                }
            }

            // Jika login berhasil, masuk ke menu admin
            if (isAuthenticated_Kelompok1_SI4804)
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
                    int choice_Kelompok1_SI4804 = int.Parse(Console.ReadLine());

                    if (choice_Kelompok1_SI4804 == 0) break; // Kembali ke menu sebelumnya

                    // Pilihan menu admin
                    switch (choice_Kelompok1_SI4804)
                    {
                        case 1:
                            Console.WriteLine("\n========== Daftar Stok ==========");
                            using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_Kelompok1_SI4804))
                            {
                                using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
                                {
                                    while (reader_Kelompok1_SI4804.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_Kelompok1_SI4804["Id"]}, Nama: {reader_Kelompok1_SI4804["Name"]}, Harga: Rp {reader_Kelompok1_SI4804["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("\nTekan Enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 2:
                            Console.Write("\nMasukkan Nama Bunga: ");
                            string newName_Kelompok1_SI4804 = Console.ReadLine();
                            Console.Write("Masukkan Harga Bunga: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice_Kelompok1_SI4804))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_Kelompok1_SI4804 = new MySqlCommand("INSERT INTO `flowers-new` (Name, Price) VALUES (@name, @price)", connection_Kelompok1_SI4804))
                            {
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@name", newName_Kelompok1_SI4804);
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@price", newPrice_Kelompok1_SI4804);
                                command_Kelompok1_SI4804.ExecuteNonQuery();
                            }
                            Console.WriteLine("\nBunga berhasil ditambahkan!");
                            Console.WriteLine("\nTekan Enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 3:
                            Console.WriteLine("\n========== Update Harga Bunga ==========");
                            DisplayAllFlowers_Kelompok1_SI4804(connection_Kelompok1_SI4804);
                            Console.Write("\nMasukkan ID Bunga yang ingin diupdate: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId_Kelompok1_SI4804))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            Console.Write("Masukkan Harga Baru: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal updatedPrice_Kelompok1_SI4804))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_Kelompok1_SI4804 = new MySqlCommand("UPDATE `flowers-new` SET Price = @price WHERE Id = @id", connection_Kelompok1_SI4804))
                            {
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@price", updatedPrice_Kelompok1_SI4804);
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@id", updateId_Kelompok1_SI4804);
                                if (command_Kelompok1_SI4804.ExecuteNonQuery() > 0)
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
                            break;

                        case 4:
                            Console.WriteLine("\n========== Hapus Bunga ==========");
                            DisplayAllFlowers_Kelompok1_SI4804(connection_Kelompok1_SI4804);
                            Console.Write("\nMasukkan ID Bunga yang ingin dihapus: ");
                            if (!int.TryParse(Console.ReadLine(), out int deleteId_Kelompok1_SI4804))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_Kelompok1_SI4804 = new MySqlCommand(" DELETE FROM `flowers-new` WHERE Id = @id", connection_Kelompok1_SI4804))
                            {
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@id", deleteId_Kelompok1_SI4804);
                                if (command_Kelompok1_SI4804.ExecuteNonQuery() > 0)
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
                            if (!int.TryParse(Console.ReadLine(), out int searchId_Kelompok1_SI4804))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT * FROM `flowers-new` WHERE Id = @id", connection_Kelompok1_SI4804))
                            {
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@id", searchId_Kelompok1_SI4804);
                                using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
                                {
                                    if (reader_Kelompok1_SI4804.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_Kelompok1_SI4804["Id"]}, Nama: {reader_Kelompok1_SI4804["Name"]}, Harga: Rp {reader_Kelompok1_SI4804["Price"]}");
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
                            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice_Kelompok1_SI4804))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT * FROM `flowers-new` WHERE Price <= @maxPrice", connection_Kelompok1_SI4804))
                            {
                                command_Kelompok1_SI4804.Parameters.AddWithValue("@maxPrice", maxPrice_Kelompok1_SI4804);
                                using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
                                {
                                    Console.WriteLine("\nHasil Filter:");
                                    while (reader_Kelompok1_SI4804.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_Kelompok1_SI4804["Id"]}, Nama: {reader_Kelompok1_SI4804["Name"]}, Harga: Rp {reader_Kelompok1_SI4804["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("Tekan Apapun untuk Kembali..");
                            Console.ReadKey();
                            break;

                        case 0:
                            Console.WriteLine("\nKembali ke menu sebelumnya..");
                            return; // Keluar dari fungsi/method yang memuat switch ini

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

    private static void DisplayAllFlowers_Kelompok1_SI4804(MySqlConnection connection_Kelompok1_SI4804)
    {
        Console.WriteLine("\n========== Daftar Stok ==========");
        using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_Kelompok1_SI4804))
        {
            using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
            {
                while (reader_Kelompok1_SI4804.Read())
                {
                    Console.WriteLine($"ID: {reader_Kelompok1_SI4804["Id"]}, Nama: {reader_Kelompok1_SI4804["Name"]}, Harga: Rp {reader_Kelompok1_SI4804["Price"]}");
                }
            }
        }
    }

    private static void PelangganMenu_Kelompok1_SI4804()
    {
        string customerName_Kelompok1_SI4804 = "";
        string customerAddress_Kelompok1_SI4804 = "";
        List<(int FlowerId_Kelompok1_SI4804, string FlowerName_Kelompok1_SI4804, int Quantity_Kelompok1_SI4804, decimal Subtotal_Kelompok1_SI4804)> cart_Kelompok1_SI4804 = new List<(int, string, int, decimal)>();

        string connectionString_Kelompok1_SI4804 = "Server=localhost;Database=florist;UserID=root;Password=;";
        using (var connection_Kelompok1_SI4804 = new MySqlConnection(connectionString_Kelompok1_SI4804))
        {
            connection_Kelompok1_SI4804.Open();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Daftar Stok ==========");
                using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_Kelompok1_SI4804))
                {
                    using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
                    {
                        while (reader_Kelompok1_SI4804.Read())
                        {
                            Console.WriteLine($"ID: {reader_Kelompok1_SI4804["Id"]}, Nama: {reader_Kelompok1_SI4804["Name"]}, Harga: Rp {reader_Kelompok1_SI4804["Price"]}");
                        }
                    }
                }

                Console.WriteLine("\n========== Tambah Pesanan ==========");
                Console.Write("Masukkan ID Bunga yang ingin dibeli: ");
                if (!int.TryParse(Console.ReadLine(), out int flowerId_Kelompok1_SI4804))
                {
                    Console.WriteLine("ID tidak valid. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                // Check if the flower exists in the database
                string flowerName_Kelompok1_SI4804 = null;
                decimal flowerPrice_Kelompok1_SI4804 = 0;
                using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT Name, Price FROM `flowers-new` WHERE Id = @id", connection_Kelompok1_SI4804))
                {
                    command_Kelompok1_SI4804.Parameters.AddWithValue("@id", flowerId_Kelompok1_SI4804);
                    using (var reader_Kelompok1_SI4804 = command_Kelompok1_SI4804.ExecuteReader())
                    {
                        if (reader_Kelompok1_SI4804.Read())
                        {
                            flowerName_Kelompok1_SI4804 = reader_Kelompok1_SI4804["Name"].ToString();
                            flowerPrice_Kelompok1_SI4804 = Convert.ToDecimal(reader_Kelompok1_SI4804["Price"]);
                        }
                    }
                }

                if (flowerName_Kelompok1_SI4804 == null)
                {
                    Console.WriteLine("ID bunga tidak ditemukan. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Masukkan jumlah: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity_Kelompok1_SI4804) || quantity_Kelompok1_SI4804 <= 0)
                {
                    Console.WriteLine("Jumlah tidak valid. Tekan Enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                decimal subtotal_Kelompok1_SI4804 = flowerPrice_Kelompok1_SI4804 * quantity_Kelompok1_SI4804;
                cart_Kelompok1_SI4804.Add((flowerId_Kelompok1_SI4804, flowerName_Kelompok1_SI4804, quantity_Kelompok1_SI4804, subtotal_Kelompok1_SI4804));
                Console.WriteLine($"Bunga {flowerName_Kelompok1_SI4804} sebanyak {quantity_Kelompok1_SI4804} berhasil ditambahkan ke keranjang.");

                Console.WriteLine("\n========== Keranjang Belanja ==========");
                foreach (var item in cart_Kelompok1_SI4804)
                {
                    Console.WriteLine($"- {item.FlowerName_Kelompok1_SI4804}, Jumlah: {item.Quantity_Kelompok1_SI4804}, Subtotal: Rp {item.Subtotal_Kelompok1_SI4804}");
                }

                Console.Write("\nIngin menambahkan bunga lain? (y/n): ");
                string addMore_Kelompok1_SI4804 = Console.ReadLine()?.ToLower();
                if (addMore_Kelompok1_SI4804 != "y")
                {
                    break;
                }
            }

            // Konfirmasi Keranjang
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Konfirmasi Keranjang ==========");
                decimal totalPrice_Kelompok1_SI4804 = 0;

                foreach (var item in cart_Kelompok1_SI4804)
                {
                    Console.WriteLine($"- {item.FlowerName_Kelompok1_SI4804}, Jumlah: {item.Quantity_Kelompok1_SI4804}, Subtotal: Rp {item.Subtotal_Kelompok1_SI4804}");
                    totalPrice_Kelompok1_SI4804 += item.Subtotal_Kelompok1_SI4804;
                }

                Console.WriteLine($"Total Harga: Rp {totalPrice_Kelompok1_SI4804}");
                Console.Write("\nApakah keranjang sudah sesuai? (y/n): ");
                string confirmation_Kelompok1_SI4804 = Console.ReadLine()?.ToLower();

                if (confirmation_Kelompok1_SI4804 == "y")
                {
                    break; // Lanjut ke detail pemesan
                }
                else if (confirmation_Kelompok1_SI4804 == "n")
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
            customerName_Kelompok1_SI4804 = Console.ReadLine();
            Console.Write("Masukkan Alamat Anda: ");
            customerAddress_Kelompok1_SI4804 = Console.ReadLine();

            // Simpan Pesanan ke Database
            using (var transaction_Kelompok1_SI4804 = connection_Kelompok1_SI4804.BeginTransaction())
            {
                try
                {
                    // Insert Order
                    decimal finalTotalPrice_Kelompok1_SI4804 = cart_Kelompok1_SI4804.Sum(item => item.Subtotal_Kelompok1_SI4804);
                    long orderId_Kelompok1_SI4804;

                    using (var command_Kelompok1_SI4804 = new MySqlCommand("INSERT INTO Orders (CustomerName, CustomerAddress, TotalPrice) VALUES (@name, @address, @totalPrice)", connection_Kelompok1_SI4804, transaction_Kelompok1_SI4804))
                    {
                        command_Kelompok1_SI4804.Parameters.AddWithValue("@name", customerName_Kelompok1_SI4804);
                        command_Kelompok1_SI4804.Parameters.AddWithValue("@address", customerAddress_Kelompok1_SI4804);
                        command_Kelompok1_SI4804.Parameters.AddWithValue("@totalPrice", finalTotalPrice_Kelompok1_SI4804);
                        command_Kelompok1_SI4804.ExecuteNonQuery();
                    }

                    // Get the last inserted OrderId
                    using (var command_Kelompok1_SI4804 = new MySqlCommand("SELECT LAST_INSERT_ID()", connection_Kelompok1_SI4804, transaction_Kelompok1_SI4804))
                    {
                        orderId_Kelompok1_SI4804 = Convert.ToInt64(command_Kelompok1_SI4804.ExecuteScalar());
                    }

                    // Insert Order Details
                    foreach (var item in cart_Kelompok1_SI4804)
                    {
                        using (var command_Kelompok1_SI4804 = new MySqlCommand("INSERT INTO OrderDetails (OrderId, FlowerId, Quantity, Subtotal) VALUES (@orderId, @flowerId, @quantity, @subtotal)", connection_Kelompok1_SI4804, transaction_Kelompok1_SI4804))
                        {
                            command_Kelompok1_SI4804.Parameters.AddWithValue("@orderId", orderId_Kelompok1_SI4804);
                            command_Kelompok1_SI4804.Parameters.AddWithValue("@flowerId", item.FlowerId_Kelompok1_SI4804);
                            command_Kelompok1_SI4804.Parameters.AddWithValue("@quantity", item.Quantity_Kelompok1_SI4804);
                            command_Kelompok1_SI4804.Parameters.AddWithValue("@subtotal", item.Subtotal_Kelompok1_SI4804);
                            command_Kelompok1_SI4804.ExecuteNonQuery();
                        }
                    }

                    transaction_Kelompok1_SI4804.Commit();
                    Console.WriteLine("\nPesanan berhasil dibuat! Terima kasih telah berbelanja.");
                    Console.WriteLine();
                    Console.WriteLine("Tekan Apapun untuk Kembali..");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    transaction_Kelompok1_SI4804.Rollback();
                    Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
                }
            }
            Console.ReadKey();
        }
    }
}
