using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

/*
Kelas SI-48-04
Kelompok 1
Anggota Kelompok:
1. Radiv Lazuardi Aziz (102022430052)
2. Lola Larasati Br Pasaribu (102022400218)
3. Sabila Martasyah (102022430005)
4. Caesya Rachma Arizna (102022400187)
5. Zhafir Akram Putra Wardana (102022400148)
*/

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("||                 Florist                  ||");
            Console.WriteLine("==============================================");
            Console.WriteLine("Pilih User: ");
            Console.WriteLine("1. Pelanggan");
            Console.WriteLine("2. Admin");
            Console.WriteLine("0. Exit");
            Console.Write("Pilihan: ");
            int PilihMenu_0401;
            if (!int.TryParse(Console.ReadLine(), out PilihMenu_0401)) // Jika pilihan bukan sama dengan angka
            {
                Console.WriteLine("Input tidak valid. Masukkan angka.");
                Console.ReadKey();
                continue;
            }
            if (PilihMenu_0401 == 1)
            {
                PelangganMenu_0401();
            }
            else if (PilihMenu_0401 == 2)
            {
                AdminMenu_0401();
            }
            else if (PilihMenu_0401 == 0)
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

    private static void AdminMenu_0401()
    {
        string connectionString_0401 = "Server=localhost;Database=florist;UserID=root;Password=;";
        using (var connection_0401 = new MySqlConnection(connectionString_0401))
        {
            connection_0401.Open();

            var adminCredentials_0401 = new (string Username, string Password)[]
            {
                ("Zhafir", "48"),
                ("Radiv", "52"),
                ("Caesya", "87"),
                ("Lola", "18"),
                ("Sabila", "5")
            };

            // Login admin
            int maxAttempts_0401 = 3; // 3 kali kesempatan
            bool isAuthenticated_0401 = false;

            for (int attempt_0401 = 1; attempt_0401 <= maxAttempts_0401; attempt_0401++)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("||              Login Admin                ||");
                Console.WriteLine("==============================================");
                Console.Write("Username: ");
                string username_0401 = Console.ReadLine();
                Console.Write("Password: ");
                string password_0401 = Console.ReadLine();

                foreach (var credential_0401 in adminCredentials_0401)
                {
                    if (credential_0401.Username.Equals(username_0401, StringComparison.OrdinalIgnoreCase) &&
                        credential_0401.Password == password_0401)
                    {
                        isAuthenticated_0401 = true;
                        Console.WriteLine("\nWelcome, Admin!");
                        Console.WriteLine("Tekan enter untuk melanjutkan..");
                        Console.ReadKey();
                        break;
                    }
                }

                if (isAuthenticated_0401)
                {
                    break;
                }
                else
                {
                    int attemptsLeft_0401 = maxAttempts_0401 - attempt_0401;
                    Console.WriteLine($"\nUsername atau Password Salah! {attemptsLeft_0401} Percobaan Lagi.");
                    if (attempt_0401 < maxAttempts_0401)
                    {
                        Console.WriteLine("Tekan enter untuk melanjutkan..");
                        Console.ReadKey();
                    }
                }
            }

            // Jika login berhasil, masuk ke menu admin
            if (isAuthenticated_0401)
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
                    Console.WriteLine("7. Urutkan Harga");
                    Console.WriteLine("0. Kembali");
                    Console.WriteLine("==============================================");
                    Console.Write("Pilihan: ");
                    int choice_0401 = int.Parse(Console.ReadLine());

                    if (choice_0401 == 0) break; // Kembali ke menu sebelumnya

                    // Pilihan menu admin
                    switch (choice_0401)
                    {
                        case 1:
                            Console.WriteLine("\n========== Daftar Stok ==========");
                            using (var command_0401 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_0401))
                            {
                                using (var reader_0401 = command_0401.ExecuteReader())
                                {
                                    while (reader_0401.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("\nTekan enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 2:
                            Console.Write("\nMasukkan Nama Bunga: ");
                            string newName_0401 = Console.ReadLine();
                            Console.Write("Masukkan Harga Bunga: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice_0401))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_0401 = new MySqlCommand("INSERT INTO `flowers-new` (Name, Price) VALUES (@name, @price)", connection_0401))
                            {
                                command_0401.Parameters.AddWithValue("@name", newName_0401);
                                command_0401.Parameters.AddWithValue("@price", newPrice_0401);
                                command_0401.ExecuteNonQuery();
                            }
                            Console.WriteLine("\nBunga berhasil ditambahkan!");
                            Console.WriteLine("\nTekan enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 3:
                            Console.WriteLine("\n========== Update Harga Bunga ==========");
                            DisplayAllFlowers_0401(connection_0401);
                            Console.Write("\nMasukkan ID Bunga yang ingin diupdate: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId_0401))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            Console.Write("Masukkan Harga Baru: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal updatedPrice_0401))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_0401 = new MySqlCommand("UPDATE `flowers-new` SET Price = @price WHERE Id = @id", connection_0401))
                            {
                                command_0401.Parameters.AddWithValue("@price", updatedPrice_0401);
                                command_0401.Parameters.AddWithValue("@id", updateId_0401);
                                if (command_0401.ExecuteNonQuery() > 0)
                                {
                                    Console.WriteLine("\nHarga berhasil diperbarui!");
                                    Console.WriteLine("Tekan enter untuk Kembali..");
                                }
                                else
                                {
                                    Console.WriteLine("\nID tidak ditemukan.");
                                    Console.WriteLine("Tekan enter untuk Kembali..");
                                }
                                Console.ReadKey();
                            }
                            break;

                        case 4:
                            Console.WriteLine("\n========== Hapus Bunga ==========");
                            DisplayAllFlowers_0401(connection_0401);
                            Console.Write("\nMasukkan ID Bunga yang ingin dihapus: ");
                            if (!int.TryParse(Console.ReadLine(), out int deleteId_0401))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_0401 = new MySqlCommand("DELETE FROM `flowers-new` WHERE Id = @id", connection_0401))
                            {
                                command_0401.Parameters.AddWithValue("@id", deleteId_0401);
                                if (command_0401.ExecuteNonQuery() > 0)
                                {
                                    Console.WriteLine("Bunga berhasil dihapus!");
                                }
                                else
                                {
                                    Console.WriteLine("ID tidak ditemukan.");
                                }
                            }
                            Console.WriteLine("Tekan enter untuk Kembali..");
                            Console.ReadKey();
                            break;

                        case 5:
                            Console.WriteLine("\n========== Cari Bunga Berdasarkan ID ==========");
                            Console.Write("\nMasukkan ID Bunga: ");
                            if (!int.TryParse(Console.ReadLine(), out int searchId_0401))
                            {
                                Console.WriteLine("ID tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_0401 = new MySqlCommand("SELECT * FROM `flowers-new` WHERE Id = @id", connection_0401))
                            {
                                command_0401.Parameters.AddWithValue("@id", searchId_0401);
                                using (var reader_0401 = command_0401.ExecuteReader())
                                {
                                    if (reader_0401.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID tidak ditemukan.");
                                    }
                                }
                            }
                            Console.WriteLine("Tekan enter untuk Kembali..");
                            Console.ReadKey();
                            break;

                        case 6:
                            Console.WriteLine("\n========== Filter Berdasarkan Harga ==========");
                            Console.Write("\nMasukkan harga maksimum: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice_0401))
                            {
                                Console.WriteLine("Harga tidak valid.");
                                Console.ReadKey();
                                break;
                            }
                            using (var command_0401 = new MySqlCommand("SELECT * FROM `flowers-new` WHERE Price <= @maxPrice", connection_0401))
                            {
                                command_0401.Parameters.AddWithValue("@maxPrice", maxPrice_0401);
                                using (var reader_0401 = command_0401.ExecuteReader())
                                {
                                    Console.WriteLine("\nHasil Filter:");
                                    while (reader_0401.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("Tekan enter untuk Kembali..");
                            Console.ReadKey();
                            break;

                        case 7:
                            Console.WriteLine("\n========== Urutkan Harga ==========");
                            Console.WriteLine("Pilih urutan harga:");
                            Console.WriteLine("1. Harga Tertinggi");
                            Console.WriteLine("2. Harga Terendah");
                            Console.Write("Pilihan: ");
                            string sortOption_0401 = Console.ReadLine();

                            string sortQuery_0401 = "";
                            if (sortOption_0401 == "1")
                            {
                                sortQuery_0401 = "SELECT * FROM `flowers-new` ORDER BY Price DESC";
                            }
                            else if (sortOption_0401 == "2")
                            {
                                sortQuery_0401 = "SELECT * FROM `flowers-new` ORDER BY Price ASC";
                            }
                            else
                            {
                                Console.WriteLine("Pilihan tidak valid.");
                                Console.ReadKey();
                                break;
                            }

                            using (var command_0401 = new MySqlCommand(sortQuery_0401, connection_0401))
                            {
                                using (var reader_0401 = command_0401.ExecuteReader())
                                {
                                    Console.WriteLine("\nHasil Urutkan Harga:");
                                    while (reader_0401.Read())
                                    {
                                        Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                                    }
                                }
                            }
                            Console.WriteLine("\nTekan enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;

                        case 0:
                            Console.WriteLine("\nKembali ke menu sebelumnya..");
                            return; // Keluar dari fungsi/method yang memuat switch ini

                        default:
                            Console.WriteLine("\nPilihan tidak valid. Silakan coba lagi.");
                            Console.WriteLine("\nTekan enter untuk kembali ke menu admin..");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }

    private static void DisplayAllFlowers_0401(MySqlConnection connection_0401)
    {
        Console.WriteLine("\n========== Daftar Stok ==========");
        using (var command_0401 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_0401))
        {
            using (var reader_0401 = command_0401.ExecuteReader())
            {
                while (reader_0401.Read())
                {
                    Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                }
            }
        }
    }

    private static void PelangganMenu_0401()
    {
        string customerName_0401 = "";
        string customerAddress_0401 = "";
        List<(int FlowerId_0401, string FlowerName_0401, int Quantity_0401, decimal Subtotal_0401)> cart_0401 = new List<(int, string, int, decimal)>();

        string connectionString_0401 = "Server=localhost;Database=florist;UserID=root;Password=;";
        using (var connection_0401 = new MySqlConnection(connectionString_0401))
        {
            connection_0401.Open();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Daftar Stok ==========");
                using (var command_0401 = new MySqlCommand("SELECT * FROM `flowers-new`", connection_0401))
                {
                    using (var reader_0401 = command_0401.ExecuteReader())
                    {
                        while (reader_0401.Read())
                        {
                            Console.WriteLine($"ID: {reader_0401["Id"]}, Nama: {reader_0401["Name"]}, Harga: Rp {reader_0401["Price"]}");
                        }
                    }
                }

                Console.WriteLine("\n========== Tambah Pesanan ==========");
                Console.Write("Masukkan ID Bunga yang ingin dibeli: ");
                if (!int.TryParse(Console.ReadLine(), out int flowerId_0401))
                {
                    Console.WriteLine("ID tidak valid. Tekan enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                
                string flowerName_0401 = null;
                decimal flowerPrice_0401 = 0;
                using (var command_0401 = new MySqlCommand("SELECT Name, Price FROM `flowers-new` WHERE Id = @id", connection_0401))
                {
                    command_0401.Parameters.AddWithValue("@id", flowerId_0401);
                    using (var reader_0401 = command_0401.ExecuteReader())
                    {
                        if (reader_0401.Read())
                        {
                            flowerName_0401 = reader_0401["Name"].ToString();
                            flowerPrice_0401 = Convert.ToDecimal(reader_0401["Price"]);
                        }
                    }
                }

                if (flowerName_0401 == null)
                {
                    Console.WriteLine("ID bunga tidak ditemukan. Tekan enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Masukkan jumlah: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity_0401) || quantity_0401 <= 0)
                {
                    Console.WriteLine("Jumlah tidak valid. Tekan enter untuk mencoba lagi..");
                    Console.ReadKey();
                    continue;
                }

                decimal subtotal_0401 = flowerPrice_0401 * quantity_0401;
                cart_0401.Add((flowerId_0401, flowerName_0401, quantity_0401, subtotal_0401));
                Console.WriteLine($"Bunga {flowerName_0401} sebanyak {quantity_0401} berhasil ditambahkan ke keranjang.");

                Console.WriteLine("\n========== Keranjang Belanja ==========");
                // Perhitungan Total Harga
                decimal totalPrice_0401 = 0;

                foreach (var item in cart_0401)
                {
                    Console.WriteLine($"- {item.FlowerName_0401}, Jumlah: {item.Quantity_0401}, Subtotal: Rp {item.Subtotal_0401}");
                    totalPrice_0401 += item.Subtotal_0401;
                }

                // Hitung PPN
                decimal tax_0401 = totalPrice_0401 * 0.11m;
                decimal finalTotalPrice_0401 = totalPrice_0401 + tax_0401;

                // Tampilkan rincian harga
                Console.WriteLine($"\nSubtotal: Rp {totalPrice_0401:F2}");
                Console.WriteLine($"PPN 11%: Rp {tax_0401:F2}");
                Console.WriteLine($"Total Harga (termasuk PPN): Rp {finalTotalPrice_0401:F2}");



                Console.Write("\nIngin menambahkan bunga lain? (y/n): ");
                string addMore_0401 = Console.ReadLine()?.ToLower();
                if (addMore_0401 != "y")
                {
                    break;
                }
            }

            // Konfirmasi Keranjang
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Konfirmasi Keranjang ==========");
                decimal totalPrice_0401 = 0;

                foreach (var item in cart_0401)
                {
                    Console.WriteLine($"- {item.FlowerName_0401}, Jumlah: {item.Quantity_0401}, Subtotal: Rp {item.Subtotal_0401}");
                    totalPrice_0401 += item.Subtotal_0401;
                }

                // Hitung PPN
                decimal tax_0401 = totalPrice_0401 * 0.11m;
                decimal finalTotalPrice_0401 = totalPrice_0401 + tax_0401;

                // Tampilkan rincian harga
                Console.WriteLine($"\nSubtotal: Rp {totalPrice_0401:F2}");
                Console.WriteLine($"PPN 11%: Rp {tax_0401:F2}");
                Console.WriteLine($"Total Harga (termasuk PPN): Rp {finalTotalPrice_0401:F2}");
                Console.Write("\nApakah keranjang sudah sesuai? (y/n): ");

                string confirmation_0401 = Console.ReadLine()?.ToLower();

                if (confirmation_0401 == "y")
                {
                    break; // Lanjut ke detail pemesan
                }
                else if (confirmation_0401 == "n")
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
            customerName_0401 = Console.ReadLine();
            Console.Write("Masukkan Alamat Anda: ");
            customerAddress_0401 = Console.ReadLine();

            // Simpan Pesanan ke Database
            using (var transaction_0401 = connection_0401.BeginTransaction())
            {
                try
                {
                    // Validasi Input Tidak Boleh Kosong
                    if (string.IsNullOrWhiteSpace(customerName_0401) || string.IsNullOrWhiteSpace(customerAddress_0401))
                    {
                        throw new Exception("Nama dan alamat pelanggan tidak boleh kosong.");
                    }

                    // Insert Order
                    decimal finalTotalPrice_0401 = cart_0401.Sum(item => item.Subtotal_0401);
                    if (finalTotalPrice_0401 <= 0)
                    {
                        throw new Exception("Total harga pesanan tidak boleh nol atau negatif.");
                    }

                    long orderId_0401;

                    using (var command_0401 = new MySqlCommand("INSERT INTO Orders (CustomerName, CustomerAddress, TotalPrice) VALUES (@name, @address, @totalPrice)", connection_0401, transaction_0401))
                    {
                        command_0401.Parameters.AddWithValue("@name", customerName_0401);
                        command_0401.Parameters.AddWithValue("@address", customerAddress_0401);
                        command_0401.Parameters.AddWithValue("@totalPrice", finalTotalPrice_0401); // Gunakan total harga termasuk PPN
                        command_0401.ExecuteNonQuery();
                    }



                    using (var command_0401 = new MySqlCommand("SELECT LAST_INSERT_ID()", connection_0401, transaction_0401))
                    {
                        orderId_0401 = Convert.ToInt64(command_0401.ExecuteScalar());
                    }

                    
                    foreach (var item in cart_0401)
                    {
                        using (var command_0401 = new MySqlCommand("INSERT INTO OrderDetails (OrderId, FlowerId, Quantity, Subtotal) VALUES (@orderId, @flowerId, @quantity, @subtotal)", connection_0401, transaction_0401))
                        {
                            command_0401.Parameters.AddWithValue("@orderId", orderId_0401);
                            command_0401.Parameters.AddWithValue("@flowerId", item.FlowerId_0401);
                            command_0401.Parameters.AddWithValue("@quantity", item.Quantity_0401);
                            command_0401.Parameters.AddWithValue("@subtotal", item.Subtotal_0401);
                            command_0401.ExecuteNonQuery();
                        }
                    }

                    transaction_0401.Commit();
                    Console.WriteLine("\nPesanan berhasil dibuat! Terima kasih telah berbelanja.");
                    Console.WriteLine();
                    Console.WriteLine("Tekan enter untuk Kembali..");
                    Console.ReadKey();
                }
                catch (MySqlException ex) // Jika koneksi ke database mati
                {
                    transaction_0401.Rollback();
                    Console.WriteLine($"Kesalahan koneksi ke database: {ex.Message}");
                }
                catch (Exception ex) // Validasi input atau error umum
                {
                    transaction_0401.Rollback();
                    Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
                }
            }
        }
    }
}


