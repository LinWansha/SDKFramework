#define USE_ENCRYPTION
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto; // For AsymmetricKeyParameter
using Org.BouncyCastle.Crypto.Parameters; // For RsaKeyParameters
using Org.BouncyCastle.Security; // For PublicKeyFactory
using Org.BouncyCastle.Asn1; // For Asn1Object and Asn1Sequence
using Org.BouncyCastle.Asn1.Pkcs; // For RsaPrivateKeyStructure

public class NetEnc
{
    //  public const int XOR_NUM = 846337046;
    public const int XOR_NUM = 13015634;
    private const char XOR_MIN = ' ';

    public static readonly int[] PUBLIC_KEY =
    {
	    15459353, 14699482, 9510360, 9512939, 14840164, 16206376, 16777020, 15519387, 15540913, 9021708, 16531103,
	    15691475, 9364641, 9499155, 8730375, 15227875, 15186144, 16201727, 14926951, 14980676, 14988025, 16242432,
	    15179675, 14860713, 16110463, 9508092, 15185015, 9560968, 16101266, 15182939, 15479662, 14976866, 15189041,
	    15418216, 13708615, 13772594, 14610275, 15317122, 15053282, 14035977, 9711931, 9512454, 16058150, 15202157,
	    9299715, 10380660, 9454526, 9726074, 16521547, 15481927, 13834833, 13730244, 15280128, 8516627, 9144702,
	    13937323, 8999405, 16151482, 9545911, 16220339, 14563335, 9925062, 15025968, 8525960, 12980336, 9822116,
	    13634636, 8650304, 15005143, 15414300, 15634571, 9004464, 15260164, 14875745, 9667924, 9154786, 16725388,
	    13225137, 15796049, 15323221, 8924816, 8871940, 9645896, 9639417, 9055316, 8394740, 13939557, 9843753, 9395292,
	    14687310, 14117875, 13898011, 8664086, 14781085, 15306324, 9512327, 14959776, 14762657, 16677199, 9809161,
	    14597427, 13872737, 8510126, 8867481, 9896302, 8627240, 15991755, 15503402, 16700777, 16740910, 14001446,
	    15640935, 8884588, 14709388, 16105002, 14679050, 16084609, 14604261, 13767039, 14557319, 15024458, 15171057,
	    9503830, 14906171, 15148799, 15197634, 16239564, 14383026, 14403711
    };
    public const int XOR_PRIVATE_NUM = 76487006;

    public static readonly int[] PRIVATE_KEY =
    {
	    77728542, 78025468, 78008745, 78508571, 77632235, 81320836, 78036524, 78470320, 78541032, 78576921, 77972678,
	    78461876, 78524272, 77597951, 79400993, 80232856, 77734707, 79284058, 81440589, 79100590, 77348868, 80273582,
	    79786400, 81155345, 80083958, 77952120, 77260594, 79561060, 81719136, 79241143, 79684776, 79109063, 81300666,
	    77392133, 79418723, 79524300, 78100549, 77385966, 79306377, 77050726, 81554999, 77809915, 81729697, 81231272,
	    78294593, 77515271, 80553083, 77707056, 77315397, 78979579, 77378139, 76999454, 78540749, 79875095, 79746575,
	    75497742, 79493539, 80257631, 81465795, 77137873, 77044633, 77092047, 79304535, 79002515, 76529140, 77251489,
	    79991896, 77618317, 81612858, 79087128, 77836021, 77425123, 79351667, 77206149, 78525374, 77979472, 80211631,
	    80141119, 79776471, 81581302, 79309151, 78403232, 77294913, 80651463, 77349562, 79500008, 81565016, 81254853,
	    79218237, 78277400, 77307595, 80609092, 81123405, 81391638, 77610543, 78131334, 80239298, 79643861, 81189138,
	    77579646, 78405389, 78556405, 81279549, 78289520, 78551460, 78521110, 79562061, 77951974, 78541212, 77664285,
	    77740289, 78038443, 79903887, 79683600, 79738297, 81586300, 79662741, 80615319, 76947383, 79200682, 79058855,
	    80663861, 79707543, 79727657, 78073583, 80582324, 80000756, 80475579, 78101405, 76523071, 79469356, 75776481,
	    76946251, 78278199, 81239563, 78455281, 77149637, 79083028, 79308225, 79808171, 78099618, 81577721, 79236972,
	    80007038, 77023417, 80179623, 78145532, 80387904, 77587428, 79638289, 79841592, 78113394, 79646716, 77741574,
	    81433721, 77488310, 79137212, 80647346, 79573539, 78362167, 78418880, 81081968, 77438736, 77430500, 79060084,
	    79797415, 81183678, 81180470, 78997671, 80571750, 79289817, 77132255, 75549556, 81656851, 79736847, 81081589,
	    77753804, 77075548, 81270967, 79737935, 78383126, 77463227, 77048282, 77034794, 78149541, 77501976, 79026472,
	    81118179, 78361029, 80352307, 80199315, 78464086, 78404472, 81761384, 76499977, 79914896, 79577095, 78017935,
	    80172561, 78566064, 79664967, 80451533, 81726843, 78290908, 81144191, 77233851, 79349253, 77424651, 77079870,
	    81282594, 79158001, 77945703, 80630530, 79914802, 81151268, 77822028, 77997149, 81133700, 81388001, 79408314,
	    77568642, 78292770, 78044536, 80362427, 80336181, 81607814, 80182762, 79388074, 80631569, 81326466, 77171701,
	    77586244, 81372397, 81144997, 81576975, 81549392, 79153822, 80474373, 79356914, 80420424, 80377626, 79844991,
	    81085928, 78532673, 80089417, 78265011, 78563419, 77126372, 77723211, 77983003, 80107669, 81143329, 77032780,
	    77627445, 78061365, 80664259, 77173144, 81639111, 81634121, 76534268, 81468015, 79004473, 81197057, 80545923,
	    76976756, 77374128, 78159320, 78253375, 81377349, 77054107, 80516108, 81398367, 77776089, 78244087, 80551079,
	    79048165, 78526785, 77013054, 81527696, 81100681, 80153202, 81675789, 77335910, 77480788, 79674959, 80562802,
	    79973206, 77003775, 78526158, 77206762, 77033471, 78396793, 78001946, 78392148, 77585248, 79415939, 77739333,
	    77157255, 76981419, 81728476, 79660601, 79865338, 76954561, 79481591, 77610002, 78312573, 79001766, 80632220,
	    81152149, 81561943, 81556617, 78140984, 77499797, 79055390, 79194769, 80313193, 81681878, 80584203, 79835259,
	    79687433, 80375992, 81623392, 81574516, 77171257, 76511866, 77820021, 78249443, 77247011, 80016538, 80096066,
	    78438311, 78433304, 78996021, 80336891, 81649198, 78008278, 77932010, 78535121, 80144969, 78291120, 78571626,
	    79812289, 81417841, 77677758, 80234344, 80100965, 79009282, 79938138, 77030858, 78412121, 78533872, 77184408,
	    77199699, 77192896, 80067010, 79553756, 79482509, 81696358, 78408155, 80389503, 81334584, 80113034, 81284000,
	    79628017, 79286350, 81219508, 77368074, 79404321, 80515726, 81724441, 79297154, 77077159, 77417417, 78411369,
	    79616546, 81281534, 77256246, 79911956, 76999380, 79815452, 81707153, 79548748, 77132828, 77550920, 78445872,
	    78003053, 79621223, 78408568, 79361340, 76517657, 79635536, 79082530, 81106749, 79245224, 77952703, 81667047,
	    77865892, 79181534, 77368718, 77667858, 76958238, 81317992, 80434794, 80479480, 77358453, 77599056, 81463766,
	    81081969, 77781797, 77422242, 77722925, 81497047, 79119193, 79785757, 78314252, 80668659, 78535861, 79306678,
	    81338911, 81379203, 79984879, 80136153, 79790403, 78043324, 78444903, 80293694, 79588552, 78971647, 78226673,
	    81170845, 81511073, 80250550, 76720420, 76739936
    };
    private static RSACryptoServiceProvider rsaPublic;
    private static RSACryptoServiceProvider rsaPrivate;

    public static void init() {
        if (rsaPublic == null)
        {
            AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(LiteralXor.dec(XOR_NUM, XOR_MIN, PUBLIC_KEY)));
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)publicKey;
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
            rsaPublic = new RSACryptoServiceProvider();
            rsaPublic.ImportParameters(rsaParameters);
        }

        if (rsaPrivate == null)
        {
            Asn1Object privKeyObj = Asn1Object.FromByteArray(Convert.FromBase64String(LiteralXor.dec(XOR_PRIVATE_NUM, XOR_MIN, PRIVATE_KEY)));
            RsaPrivateKeyStructure privStruct = RsaPrivateKeyStructure.GetInstance((Asn1Sequence)privKeyObj);
            // Conversion from BouncyCastle to .Net framework types
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = privStruct.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = privStruct.PublicExponent.ToByteArrayUnsigned();
            rsaParameters.D = privStruct.PrivateExponent.ToByteArrayUnsigned();
            rsaParameters.P = privStruct.Prime1.ToByteArrayUnsigned();
            rsaParameters.Q = privStruct.Prime2.ToByteArrayUnsigned();
            rsaParameters.DP = privStruct.Exponent1.ToByteArrayUnsigned();
            rsaParameters.DQ = privStruct.Exponent2.ToByteArrayUnsigned();
            rsaParameters.InverseQ = privStruct.Coefficient.ToByteArrayUnsigned();
            rsaPrivate = new RSACryptoServiceProvider();
            rsaPrivate.ImportParameters(rsaParameters);
        }
    }

    public static byte[] encContent(string content) {
        #if USE_ENCRYPTION
        init();
		
        RijndaelManaged rijndaelManaged = new RijndaelManaged
		{
			Padding = PaddingMode.Zeros,
			Mode = CipherMode.CFB,
			KeySize = 128,
			BlockSize = 128
		};
		rijndaelManaged.GenerateKey ();
		rijndaelManaged.GenerateIV ();
        byte[] bKey = rsaPublic.Encrypt(rijndaelManaged.Key, true);
        MemoryStream msEncrypt = new MemoryStream ();
		msEncrypt.Write (bKey, 0, bKey.Length);
        byte[] counter = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        Aes128CounterMode am = new Aes128CounterMode(counter);
        ICryptoTransform encryptor = am.CreateEncryptor (rijndaelManaged.Key, null);
		CryptoStream csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write);
		byte[] bytes = Encoding.UTF8.GetBytes (content);
		csEncrypt.Write (bytes, 0, bytes.Length);
		csEncrypt.FlushFinalBlock ();
		return msEncrypt.ToArray();
		#else
		return Encoding.UTF8.GetBytes (content);
		#endif
	}

	public static string decContent(byte[] content) {
        #if USE_ENCRYPTION
        init();

		MemoryStream msDecrypt = new MemoryStream (content);
        byte[] encKey = new byte[64];
        msDecrypt.Read(encKey, 0, encKey.Length);
        byte[] aesKey = rsaPrivate.Decrypt(encKey, true);
		byte[] bytes = new byte[content.Length - encKey.Length];
        byte[] counter = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        Aes128CounterMode am = new Aes128CounterMode(counter);
        ICryptoTransform decryptor = am.CreateDecryptor (aesKey, null);
		CryptoStream csDecrypt = new CryptoStream (msDecrypt, decryptor, CryptoStreamMode.Read);
		csDecrypt.Read (bytes, 0, bytes.Length);
		return Encoding.UTF8.GetString (bytes);
		#else
		return Encoding.UTF8.GetString (content);
		#endif
	}

	public static string getContentType() {
		#if USE_ENCRYPTION
		return "application/octet-stream";
		#else
		return "application/json";
		#endif
	}

}
