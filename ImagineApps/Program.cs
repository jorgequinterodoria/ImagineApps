using System.Globalization;
using Microsoft.Data.SqlClient;
class Program
{
    static Dictionary<string, string> currencyRules = new Dictionary<string, string>
    {
        { "1001395", "EC$" },
        { "1026780", "EC$" },
        { "1502764", "$" },
        { "2045326", "$" },
    };

    static string GetCurrencyId(string accountId)
    {
        if (currencyRules.ContainsKey(accountId))
        {
            return currencyRules[accountId];
        }
        return "";
    }

    static Bank GetBankInfo(string connectionString, int bankId)
    {
        Bank bankInfo = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Bank_Name, Address_1, Address_2 FROM dbo.Banks WHERE Bank_ID = @BankId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BankId", bankId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bankInfo = new Bank
                        {
                            Bank_Name = reader["Bank_Name"].ToString(),
                            Address_1 = reader["Address_1"].ToString(),
                            Address_2 = reader["Address_2"].ToString()
                        };
                    }
                }
            }
        }

        return bankInfo;
    }

    static void Main()
    {
        string connectionString = "Server=localhost;Database=CheckPlus_235;user id=sa;password=reallyStrongPwd123;TrustServerCertificate=True;Trusted_Connection=False;MultipleActiveResultSets=true";

        using (StreamReader reader = new StreamReader("CHEQUE_ap.TXT"))
        {
            string today = DateTime.Now.ToString("ddMMyyyy");
            using (StreamWriter writer = new StreamWriter($"CHECK_AFT_DATE{today}.TXT"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    char recordType = line[0];

                    if (recordType == 'H')
                    {
                        string checkNumber = line.Substring(1, 8).Trim();
                        string bankId = line.Substring(9, 10).Trim();
                        string accountId = line.Substring(18, 15).Trim();
                        DateTime checkDate = DateTime.ParseExact(line.Substring(34, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string formattedCheckDate = checkDate.ToString("dd MM yyyy");
                        string currencyId = GetCurrencyId(accountId);
                        string payeeName1 = line.Substring(53, 35).Trim();
                        string payeeName2 = line.Substring(88, 35).Trim();
                        string address1 = line.Substring(123, 35).Trim();
                        string address2 = line.Substring(158, 35).Trim();
                        string address3 = line.Substring(193, 35).Trim();
                        string address4 = line.Substring(228, 35).Trim();
                        string address5 = line.Substring(263, 35).Trim();
                        string checkAmount = line.Substring(531, 14).Trim();
                        string payorId = line.Substring(545, 10).Trim();
                        string amountString = line.Substring(590, 120).Trim();
                        string bankName = GetBankInfo(connectionString, Convert.ToInt16(bankId)).Bank_Name;
                        string bankAddress1 = GetBankInfo(connectionString, Convert.ToInt16(bankId)).Address_1;
                        string bankAddress2 = GetBankInfo(connectionString, Convert.ToInt16(bankId)).Address_2;
                        string outputLine = $"H~{checkNumber}~{bankName}~{bankAddress1}~{bankAddress2}~{accountId}~{formattedCheckDate}~{currencyId}~{payeeName1}~{payeeName2}~{address1}~{address2}~{address3}~{address4}~{address5}~{checkAmount}~{payorId}~{amountString}";
                        writer.WriteLine(outputLine);
                    }
                    else if (recordType == 'D')
                    {
                        string checkNumber = line.Substring(1, 8).Trim();
                        string bankId = line.Substring(9, 10).Trim();
                        string bankAccountNo = line.Substring(19, 15).Trim();
                        DateTime checkDate = DateTime.ParseExact(line.Substring(34, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string formattedCheckDate = checkDate.ToString("dd MM yyyy");
                        string invoiceNumber = line.Substring(44, 30).Trim();
                        DateTime invoiceDate = DateTime.ParseExact(line.Substring(74, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string formattedInvoiceDate = invoiceDate.ToString("dd MM yyyy");
                        string voucherNumber = line.Substring(84, 16).Trim();
                        DateTime voucherDate = DateTime.ParseExact(line.Substring(100, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string formattedVoucherDate = voucherDate.ToString("dd MM yyyy");
                        string grossAmount = line.Substring(110, 14).Trim();
                        string discountAmount = line.Substring(124, 14).Trim();
                        string netAmount = line.Substring(138, 14).Trim();
                        string concept = line.Substring(152, 30).Trim();
                        string benefitDescription = "";
                        if (182 >= 0 && 204 <= line.Length)
                        {
                            benefitDescription = line.Substring(182, 22).Trim();
                        }
                        string outputLine = $"D~{checkNumber}~{bankId}~{bankAccountNo}~{formattedCheckDate}~{invoiceNumber}~{formattedInvoiceDate}~{voucherNumber}~{formattedVoucherDate}~{grossAmount}~{discountAmount}~{netAmount}~{concept}~{benefitDescription}";
                        writer.WriteLine(outputLine);
                    }
                }
            }
        }
    }
}


class Bank
{
    public string Bank_Name { get; set; }
    public string? Address_1 { get; set; }
    public string? Address_2 { get; set; }
}