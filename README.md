# ImagineApps

INSTRUCTIONS:

The purpose of this test is to create a program to transform the source file named: “CHEQUE_ap.TXT”
to resulting file name: “CHECK_AFT_DATE(Date today DDMMYYY).TXT”
The source file format is fixed-width. The program will open and read this file to get the columns
information according to the mapping file.
Mapping file name: “Mapping_Anguilla.xlsx”
In this file you can see two tables: HEADER and DETAILS with their corresponding columns and positions.
The Header information in the source file will start with letter “H”
The details information in the source file will start with letter “D”
In addition, there are some validations to include in the program logic to create the resulting file.
To get bank information you will need to query the table name Banks in the database
“CheckPlus_235_SSAnguilla”
As you can see The output file structure is also described and it will be separated by symbol “~” and the
output file name should be CHECK_AFT_DATE(Date today DDMMYYY).TXT
