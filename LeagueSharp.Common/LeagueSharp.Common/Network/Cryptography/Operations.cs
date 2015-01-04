using System.Collections.Generic;
using System.Linq;

namespace LeagueSharp.Network.Cryptography
{
    public class Operations : IOperation
    {
        private static readonly Dictionary<uint, Operations> CryptOperations = new Dictionary<uint, Operations>();
        private readonly IOperation[] operations;

        public Operations(params IOperation[] operations)
        {
            this.operations = operations;
        }

        static Operations()
        {
            #region Operations
            CryptOperations.Add(0x10523D0F, new Operations(new Rol(2), new Sub(55)));
            CryptOperations.Add(0x12CF5832, new Operations(new Ror(2), new Ror(1), new Add(79), new Xor(11), new Rol(4), new Xor(161)));
            CryptOperations.Add(0x13072D16, new Operations(new Rol(3), new Sub(27), new Xor(151), new Sub(49), new Rol(2)));
            CryptOperations.Add(0x130C703C, new Operations(new Ror(1), new Xor(142), new Rol(2), new Sub(97), new Rol(1), new Rol(1)));
            CryptOperations.Add(0x1337911B, new Operations(new Sub(95), new Xor(149), new Sub(68), new Xor(128), new Add(82)));
            CryptOperations.Add(0x13E55E0C, new Operations(new Ror(2), new Rol(3), new Add(60), new Rol(2), new Xor(55), new Ror(2), new Xor(212), new Sub(112)));
            CryptOperations.Add(0x14533E10, new Operations(new Xor(51), new Rol(4), new Add(67), new Rol(2), new Xor(58), new Ror(2)));
            CryptOperations.Add(0x15952FDB, new Operations(new Add(49), new Ror(3), new Sub(106), new Rol(3), new Add(104), new Ror(2), new Rol(1)));
            CryptOperations.Add(0x15E7C063, new Operations(new Xor(142), new Add(110), new Rol(2), new Ror(2), new Rol(2), new Xor(122)));
            CryptOperations.Add(0x1600C79A, new Operations(new Xor(18), new Add(47), new Xor(100), new Ror(3), new Sub(71)));
            CryptOperations.Add(0x163FBC87, new Operations(new Ror(1), new Xor(56), new Rol(2), new Ror(3), new Rol(4), new Xor(226)));
            CryptOperations.Add(0x173EDB9, new Operations(new Sub(76), new Ror(1), new Xor(112), new Add(66), new Rol(2), new Ror(2), new Ror(3)));
            CryptOperations.Add(0x1B7CEFB2, new Operations(new Add(121), new Rol(1), new Rol(1), new Ror(3), new Ror(4), new Xor(39)));
            CryptOperations.Add(0x1BF4047, new Operations(new Xor(95), new Rol(2), new Sub(85), new Ror(2), new Rol(2), new Sub(61)));
            CryptOperations.Add(0x1C500FE6, new Operations(new Ror(1), new Add(33), new Rol(3), new Add(20), new Xor(196), new Ror(1)));
            CryptOperations.Add(0x20251AD, new Operations(new Xor(151), new Sub(72), new Ror(1), new Dec(), new Rol(3), new Add(72), new Ror(1)));
            CryptOperations.Add(0x208BBE8F, new Operations(new Sub(26), new Xor(127), new Rol(1), new Ror(3), new Xor(24), new Sub(44)));
            CryptOperations.Add(0x20D2C067, new Operations(new Ror(1), new Sub(18), new Ror(4), new Ror(1), new Add(103), new Xor(163), new Add(20)));
            CryptOperations.Add(0x20D87D54, new Operations(new Sub(100), new Ror(3), new Xor(245), new Sub(41), new Rol(2), new Add(119), new Rol(1)));
            CryptOperations.Add(0x212E9196, new Operations(new Rol(1), new Rol(3), new Ror(1), new Rol(3), new Rol(1), new Xor(173)));
            CryptOperations.Add(0x217581E8, new Operations(new Add(14), new Xor(200), new Add(112), new Ror(4)));
            CryptOperations.Add(0x21909EDC, new Operations(new Add(120), new Xor(235)));
            CryptOperations.Add(0x21B37271, new Operations(new Ror(2), new Sub(124), new Ror(2), new Xor(188), new Rol(2)));
            CryptOperations.Add(0x21BD274B, new Operations(new Ror(3), new Xor(201), new Add(49), new Xor(6), new Add(100), new Rol(1), new Rol(4)));
            CryptOperations.Add(0x229345B8, new Operations(new Xor(166), new Rol(4), new Add(51), new Xor(182), new Ror(2), new Add(8)));
            CryptOperations.Add(0x23559146, new Operations(new Rol(1), new Add(77), new Ror(3), new Sub(60)));
            CryptOperations.Add(0x23B0AA33, new Operations(new Sub(98), new Ror(1), new Xor(57), new Rol(1), new Rol(1), new Add(125), new Ror(4)));
            CryptOperations.Add(0x23E793E5, new Operations(new Ror(1), new Rol(1), new Ror(2), new Ror(4), new Add(89), new Xor(238)));
            CryptOperations.Add(0x2434C30D, new Operations(new Ror(1), new Xor(248), new Sub(60), new Rol(2), new Xor(178), new Add(111), new Xor(99)));
            CryptOperations.Add(0x248A4248, new Operations(new Sub(114), new Ror(3), new Add(92), new Rol(1)));
            CryptOperations.Add(0x27AD44AD, new Operations(new Xor(179), new Ror(3), new Xor(42), new Rol(3), new Sub(105), new Rol(1), new Xor(51), new Sub(125)));
            CryptOperations.Add(0x284F0E6, new Operations(new Rol(3), new Xor(14), new Ror(1), new Sub(43)));
            CryptOperations.Add(0x29127EEB, new Operations(new Add(72), new Ror(2), new Add(85), new Xor(194), new Ror(3), new Add(54)));
            CryptOperations.Add(0x29EC0F8D, new Operations(new Rol(4), new Ror(2), new Sub(108), new Ror(2), new Add(94), new Rol(1), new Rol(1), new Xor(195)));
            CryptOperations.Add(0x2A35B810, new Operations(new Rol(3), new Xor(183), new Sub(87), new Rol(1), new Ror(4), new Add(70)));
            CryptOperations.Add(0x2BFFADDB, new Operations(new Add(98), new Ror(3), new Sub(14), new Ror(1), new Rol(4), new Add(94)));
            CryptOperations.Add(0x2C414950, new Operations(new Rol(1), new Add(38), new Ror(3), new Rol(3), new Rol(3)));
            CryptOperations.Add(0x2C891D05, new Operations(new Rol(3), new Rol(2), new Add(86)));
            CryptOperations.Add(0x2D7C723B, new Operations(new Xor(159), new Add(116), new Rol(3), new Ror(3), new Sub(83)));
            CryptOperations.Add(0x2E7223DD, new Operations(new Sub(2), new Rol(2), new Add(42)));
            CryptOperations.Add(0x2F5CF1D9, new Operations(new Rol(1), new Rol(2), new Xor(143), new Ror(1), new Xor(66)));
            CryptOperations.Add(0x3116989D, new Operations(new Sub(97), new Xor(254), new Sub(104), new Xor(154), new Add(5)));
            CryptOperations.Add(0x3130531C, new Operations(new Ror(1), new Sub(70), new Rol(4), new Xor(36), new Sub(124)));
            CryptOperations.Add(0x31BBBD47, new Operations(new Sub(31), new Rol(2), new Rol(4), new Add(95), new Rol(3), new Rol(3), new Xor(198)));
            CryptOperations.Add(0x3276C2F, new Operations(new Xor(214), new Add(18), new Rol(2), new Ror(1), new Ror(3), new Sub(20), new Xor(13)));
            CryptOperations.Add(0x347981D1, new Operations(new Sub(115), new Xor(201), new Sub(45), new Ror(2)));
            CryptOperations.Add(0x38170E44, new Operations(new Add(16), new Ror(2), new Sub(128), new Xor(190), new Sub(5)));
            CryptOperations.Add(0x38223C01, new Operations(new Xor(81), new Add(106), new Xor(179)));
            CryptOperations.Add(0x395B2694, new Operations(new Ror(1), new Sub(21), new Xor(208), new Sub(66), new Ror(3), new Xor(216), new Add(22)));
            CryptOperations.Add(0x3A068D76, new Operations(new Ror(3), new Rol(3), new Xor(8), new Ror(2), new Xor(102)));
            CryptOperations.Add(0x3CC45AED, new Operations(new Rol(2), new Ror(1), new Add(73), new Ror(1), new Xor(205), new Rol(4), new Add(88)));
            CryptOperations.Add(0x3CFC4C03, new Operations(new Xor(181), new Sub(59), new Xor(65), new Ror(4), new Ror(1), new Ror(2)));
            CryptOperations.Add(0x3E2D5205, new Operations(new Add(8), new Ror(4), new Sub(67), new Ror(3), new Xor(189), new Ror(4)));
            CryptOperations.Add(0x3F4EE101, new Operations(new Ror(2), new Ror(4), new Rol(1), new Xor(240), new Add(102), new Xor(110), new Add(18), new Xor(242)));
            CryptOperations.Add(0x3FAF30F9, new Operations(new Xor(119), new Ror(2), new Xor(195), new Ror(1), new Xor(218), new Ror(1)));
            CryptOperations.Add(0x40231434, new Operations(new Sub(110), new Ror(4)));
            CryptOperations.Add(0x403741B0, new Operations(new Ror(1), new Xor(91), new Add(71), new Rol(2), new Xor(15)));
            CryptOperations.Add(0x40579860, new Operations(new Sub(94), new Rol(4), new Xor(127), new Add(88), new Ror(3), new Ror(1), new Rol(2)));
            CryptOperations.Add(0x41026105, new Operations(new Ror(1), new Ror(1), new Add(77), new Rol(4), new Sub(10), new Ror(1)));
            CryptOperations.Add(0x41659787, new Operations(new Sub(68), new Rol(1), new Xor(218), new Sub(15), new Xor(166)));
            CryptOperations.Add(0x4227B81B, new Operations(new Rol(3), new Rol(1), new Add(82), new Rol(2), new Add(67), new Rol(1)));
            CryptOperations.Add(0x43C7107D, new Operations(new Ror(4), new Rol(3), new Xor(114), new Rol(2), new Add(58), new Rol(3), new Sub(61)));
            CryptOperations.Add(0x44755407, new Operations(new Xor(151), new Ror(2), new Ror(4), new Add(54), new Rol(2), new Ror(1), new Ror(1)));
            CryptOperations.Add(0x448036C1, new Operations(new Ror(3), new Rol(1), new Xor(121)));
            CryptOperations.Add(0x45EFA428, new Operations(new Sub(118), new Xor(58), new Sub(99), new Xor(253)));
            CryptOperations.Add(0x46DB8C03, new Operations(new Xor(93), new Add(37), new Xor(25), new Rol(1), new Sub(123), new Rol(1)));
            CryptOperations.Add(0x4746F3C4, new Operations(new Rol(3), new Add(64), new Ror(3), new Add(117), new Xor(106)));
            CryptOperations.Add(0x4A6D0CA6, new Operations(new Rol(1), new Add(86), new Ror(1), new Sub(96), new Xor(154), new Add(6), new Xor(174)));
            CryptOperations.Add(0x4C6EE815, new Operations(new Add(14), new Ror(2), new Sub(42), new Rol(4), new Xor(62)));
            CryptOperations.Add(0x4C813BCD, new Operations(new Ror(3), new Sub(116), new Rol(1), new Xor(187), new Rol(4), new Add(72)));
            CryptOperations.Add(0x4D47A944, new Operations(new Ror(2), new Ror(2), new Rol(2), new Xor(13), new Add(4), new Xor(29)));
            CryptOperations.Add(0x4D8B6355, new Operations(new Add(81), new Xor(252), new Add(3), new Rol(3)));
            CryptOperations.Add(0x4E6CE5A, new Operations(new Sub(77), new Xor(71), new Add(73), new Ror(2), new Rol(2), new Rol(2), new Add(33)));
            CryptOperations.Add(0x4FC86601, new Operations(new Ror(2), new Sub(127), new Rol(2), new Rol(3), new Xor(3), new Rol(3), new Xor(92)));
            CryptOperations.Add(0x50B896F2, new Operations(new Xor(158), new Add(107), new Xor(91), new Sub(105), new Xor(252)));
            CryptOperations.Add(0x50BBBD28, new Operations(new Rol(3), new Sub(48), new Rol(4), new Ror(4), new Rol(3), new Rol(1), new Ror(1), new Xor(131)));
            CryptOperations.Add(0x51BC2F14, new Operations(new Sub(20), new Xor(198), new Rol(1), new Xor(184), new Sub(58), new Rol(4)));
            CryptOperations.Add(0x5216F009, new Operations(new Ror(1), new Xor(46), new Add(83), new Rol(3), new Sub(127)));
            CryptOperations.Add(0x539AC989, new Operations(new Xor(236), new Ror(1), new Add(31), new Xor(44), new Rol(1), new Xor(94), new Rol(1)));
            CryptOperations.Add(0x543FFA4, new Operations(new Xor(163), new Add(92), new Xor(94), new Not(), new Ror(3), new Rol(2)));
            CryptOperations.Add(0x552D8C04, new Operations(new Xor(144), new Sub(9), new Ror(1), new Rol(1), new Rol(4)));
            CryptOperations.Add(0x55698A23, new Operations(new Rol(1), new Sub(42), new Rol(2), new Ror(4), new Ror(2), new Xor(3)));
            CryptOperations.Add(0x556D8FA, new Operations(new Ror(1), new Sub(104), new Ror(3), new Rol(3), new Xor(85), new Ror(3), new Rol(2)));
            CryptOperations.Add(0x560279AA, new Operations(new Sub(8), new Xor(170), new Rol(1), new Xor(234), new Sub(6), new Xor(58)));
            CryptOperations.Add(0x56525DA4, new Operations(new Ror(2), new Add(17), new Rol(3), new Sub(66), new Rol(1)));
            CryptOperations.Add(0x58AF03EF, new Operations(new Xor(117), new Sub(91), new Rol(2), new Sub(24), new Rol(3), new Sub(82)));
            CryptOperations.Add(0x59DFF8BC, new Operations(new Rol(3), new Add(2), new Ror(1), new Sub(73), new Xor(249)));
            CryptOperations.Add(0x5A6BEB26, new Operations(new Add(34), new Ror(3), new Add(66), new Ror(2), new Xor(23)));
            CryptOperations.Add(0x5B2FC082, new Operations(new Sub(102), new Ror(3), new Rol(3), new Xor(125), new Rol(1), new Ror(2), new Rol(1), new Xor(239)));
            CryptOperations.Add(0x5D5944DB, new Operations(new Ror(2), new Rol(1), new Sub(127), new Xor(65), new Ror(3)));
            CryptOperations.Add(0x5DE1C43F, new Operations(new Add(122), new Xor(228), new Sub(62), new Xor(180), new Rol(3), new Add(107)));
            CryptOperations.Add(0x5FC2EA0B, new Operations(new Sub(88), new Xor(68), new Add(14), new Ror(1), new Rol(3)));
            CryptOperations.Add(0x5FF3FEA6, new Operations(new Rol(1), new Sub(62), new Rol(2), new Xor(169), new Sub(71), new Xor(247), new Sub(8)));
            CryptOperations.Add(0x610075CE, new Operations(new Rol(4), new Rol(1), new Ror(2), new Xor(31), new Add(73), new Ror(3)));
            CryptOperations.Add(0x630D6200, new Operations(new Rol(3), new Ror(2), new Rol(1), new Rol(2), new Sub(82), new Ror(2)));
            CryptOperations.Add(0x634FDF6D, new Operations(new Rol(1), new Xor(201), new Ror(3), new Xor(6), new Ror(2), new Xor(237)));
            CryptOperations.Add(0x63CCE8AE, new Operations(new Xor(45), new Sub(107), new Ror(2), new Add(11), new Xor(144), new Rol(3), new Xor(65)));
            CryptOperations.Add(0x6447FF89, new Operations(new Xor(103), new Sub(33), new Rol(2), new Ror(1), new Add(60)));
            CryptOperations.Add(0x6501D62E, new Operations(new Xor(148), new Rol(3), new Xor(121), new Rol(2), new Rol(1), new Xor(35), new Add(34)));
            CryptOperations.Add(0x69337808, new Operations(new Ror(2), new Ror(3), new Add(126), new Xor(149), new Add(31)));
            CryptOperations.Add(0x6BF315F0, new Operations(new Add(105), new Ror(4), new Add(5), new Ror(1), new Xor(154)));
            CryptOperations.Add(0x6C1273D4, new Operations(new Add(77), new Ror(1), new Ror(1), new Ror(2), new Add(77)));
            CryptOperations.Add(0x6D266FDE, new Operations(new Xor(103), new Ror(3), new Xor(13), new Rol(3), new Sub(123), new Rol(1), new Add(19)));
            CryptOperations.Add(0x6E879CC, new Operations(new Xor(21), new Ror(1), new Rol(1), new Add(16), new Rol(2), new Rol(1), new Ror(1), new Sub(57)));
            CryptOperations.Add(0x6E8A98FC, new Operations(new Inc(), new Xor(181), new Rol(4), new Ror(2), new Xor(22), new Sub(37)));
            CryptOperations.Add(0x6EEED20E, new Operations(new Add(53), new Ror(1), new Add(71), new Xor(186)));
            CryptOperations.Add(0x6EFF399, new Operations(new Xor(224), new Add(33), new Rol(1), new Sub(128), new Xor(241), new Rol(2)));
            CryptOperations.Add(0x6F3B6084, new Operations(new Sub(20), new Xor(28), new Add(77), new Ror(3), new Xor(125)));
            CryptOperations.Add(0x7038FC57, new Operations(new Add(122), new Xor(244), new Ror(3), new Xor(37), new Ror(3), new Sub(101)));
            CryptOperations.Add(0x70E38D49, new Operations(new Add(30), new Ror(3), new Add(117), new Xor(251), new Ror(1), new Sub(37)));
            CryptOperations.Add(0x727C1507, new Operations(new Xor(221), new Add(42), new Rol(3), new Xor(84), new Add(48), new Xor(40)));
            CryptOperations.Add(0x74305688, new Operations(new Add(33), new Xor(72), new Sub(84), new Xor(174), new Add(36), new Xor(77)));
            CryptOperations.Add(0x758AE503, new Operations(new Add(77), new Xor(171), new Ror(2), new Xor(243), new Rol(4), new Sub(14), new Xor(19)));
            CryptOperations.Add(0x767C824E, new Operations(new Ror(2), new Add(33), new Xor(6), new Rol(4), new Sub(86), new Xor(57), new Sub(21)));
            CryptOperations.Add(0x7AB206CB, new Operations(new Add(117), new Xor(248), new Ror(3), new Add(105), new Ror(1), new Rol(2)));
            CryptOperations.Add(0x7ABEE945, new Operations(new Ror(2), new Xor(229), new Add(100), new Rol(3), new Sub(87)));
            CryptOperations.Add(0x7BC1E59B, new Operations(new Xor(98), new Ror(2), new Add(114), new Xor(153), new Add(106)));
            CryptOperations.Add(0x7C3788CB, new Operations(new Rol(2), new Xor(165), new Ror(4), new Add(112), new Ror(3), new Add(15), new Ror(4)));
            CryptOperations.Add(0x7EB54092, new Operations(new Rol(3), new Add(95), new Ror(3), new Sub(2), new Rol(2), new Ror(3), new Add(35)));
            CryptOperations.Add(0x7F4B0895, new Operations(new Add(43), new Ror(1)));
            CryptOperations.Add(0x7F946157, new Operations(new Ror(1), new Xor(7), new Add(19), new Rol(2), new Ror(2)));
            CryptOperations.Add(0x80D21402, new Operations(new Xor(244), new Rol(2), new Rol(3), new Sub(75), new Ror(3), new Xor(76)));
            CryptOperations.Add(0x80FA343E, new Operations(new Add(33), new Xor(142), new Ror(3), new Ror(3), new Add(101), new Rol(1), new Ror(1)));
            CryptOperations.Add(0x80FE4BFD, new Operations(new Ror(2), new Ror(2), new Sub(34), new Xor(240), new Rol(1), new Sub(15)));
            CryptOperations.Add(0x824B4514, new Operations(new Sub(106), new Ror(3), new Sub(27), new Xor(191), new Rol(3), new Xor(88), new Rol(2)));
            CryptOperations.Add(0x82F1813E, new Operations(new Ror(1), new Add(12), new Ror(2), new Ror(2), new Xor(44), new Sub(11)));
            CryptOperations.Add(0x838FAF46, new Operations(new Rol(1), new Add(8), new Xor(160), new Add(54), new Xor(96), new Sub(52)));
            CryptOperations.Add(0x842A6B66, new Operations(new Xor(127), new Ror(3), new Xor(185), new Add(28)));
            CryptOperations.Add(0x8581A7AF, new Operations(new Sub(96), new Xor(55), new Rol(1), new Xor(50)));
            CryptOperations.Add(0x8689FDC3, new Operations(new Rol(3), new Xor(179), new Ror(3), new Xor(11), new Add(27), new Ror(1), new Xor(80)));
            CryptOperations.Add(0x87055A92, new Operations(new Rol(3), new Rol(2), new Xor(107), new Sub(38), new Xor(131), new Rol(2)));
            CryptOperations.Add(0x87CFCD92, new Operations(new Ror(3), new Rol(3), new Rol(3), new Sub(80), new Rol(2), new Sub(94)));
            CryptOperations.Add(0x87D6B261, new Operations(new Rol(2), new Add(43), new Ror(2), new Sub(79), new Ror(1), new Xor(147), new Ror(2)));
            CryptOperations.Add(0x8808EA67, new Operations(new Xor(98), new Rol(3), new Ror(3), new Rol(4), new Xor(11), new Add(107)));
            CryptOperations.Add(0x8969FEC8, new Operations(new Xor(172), new Add(71), new Ror(2), new Sub(12)));
            CryptOperations.Add(0x897C552D, new Operations(new Add(119), new Ror(1), new Rol(4), new Xor(211), new Rol(2), new Rol(3), new Rol(1)));
            CryptOperations.Add(0x8B2ED7ED, new Operations(new Sub(21), new Rol(3), new Ror(1), new Rol(3), new Add(104), new Rol(3)));
            CryptOperations.Add(0x8C0128AE, new Operations(new Ror(3), new Rol(2), new Rol(3), new Ror(3), new Add(79), new Rol(3)));
            CryptOperations.Add(0x8CC129C1, new Operations(new Rol(2), new Sub(36), new Rol(2), new Add(28), new Rol(2)));
            CryptOperations.Add(0x8D111550, new Operations(new Sub(126), new Rol(1), new Add(105), new Rol(4), new Add(22), new Xor(242)));
            CryptOperations.Add(0x8D8AFC5F, new Operations(new Xor(144), new Ror(3), new Xor(2), new Add(81)));
            CryptOperations.Add(0x8E3A9828, new Operations(new Xor(122), new Rol(3), new Add(28), new Xor(85)));
            CryptOperations.Add(0x8E3EF80, new Operations(new Rol(1), new Rol(2), new Rol(3), new Add(120), new Rol(2), new Add(76)));
            CryptOperations.Add(0x94B0B2A, new Operations(new Add(45), new Ror(2), new Rol(1), new Ror(2), new Xor(165), new Ror(3), new Ror(1)));
            CryptOperations.Add(0x966AE3DC, new Operations(new Ror(4), new Ror(3), new Ror(1), new Ror(4)));
            CryptOperations.Add(0x9685CDF3, new Operations(new Xor(4), new Sub(51), new Ror(2), new Sub(105), new Rol(1), new Rol(3), new Ror(1)));
            CryptOperations.Add(0x97A3DC16, new Operations(new Ror(3), new Add(13), new Rol(3), new Ror(2), new Xor(84), new Rol(3), new Xor(54), new Rol(2)));
            CryptOperations.Add(0x97A569A7, new Operations(new Sub(85), new Rol(3), new Xor(239), new Add(19), new Ror(2), new Xor(227)));
            CryptOperations.Add(0x988400A9, new Operations(new Sub(78), new Rol(1), new Ror(3), new Xor(11), new Rol(3), new Xor(150), new Sub(44), new Xor(5)));
            CryptOperations.Add(0x988EDF01, new Operations(new Sub(99), new Xor(176), new Sub(8), new Rol(3), new Ror(2)));
            CryptOperations.Add(0x98AAB417, new Operations(new Ror(4), new Xor(156), new Ror(3), new Add(49), new Rol(1), new Sub(27)));
            CryptOperations.Add(0x9AC7D8F6, new Operations(new Rol(2), new Add(38), new Xor(40), new Rol(2), new Ror(3), new Add(40)));
            CryptOperations.Add(0x9AEF8FA0, new Operations(new Sub(85), new Ror(2), new Add(105), new Xor(133), new Ror(4), new Add(27)));
            CryptOperations.Add(0x9B731E1B, new Operations(new Xor(204), new Rol(3), new Xor(79), new Sub(101)));
            CryptOperations.Add(0x9C0CE83B, new Operations(new Rol(1), new Rol(3), new Ror(1), new Xor(209), new Ror(1), new Rol(3)));
            CryptOperations.Add(0x9D742F47, new Operations(new Ror(2), new Ror(2), new Ror(2), new Add(70), new Xor(141)));
            CryptOperations.Add(0x9DDFCF4D, new Operations(new Add(8), new Rol(2), new Ror(2), new Ror(1), new Sub(55), new Xor(79), new Ror(3)));
            CryptOperations.Add(0x9FD5C101, new Operations(new Xor(3), new Ror(1), new Add(54), new Xor(250), new Add(22), new Xor(239), new Sub(31)));
            CryptOperations.Add(0xA1B4ECD3, new Operations(new Xor(136), new Rol(1), new Sub(46), new Rol(2), new Xor(21), new Rol(1)));
            CryptOperations.Add(0xA22233E4, new Operations(new Rol(1), new Xor(60), new Add(93), new Ror(3), new Sub(96)));
            CryptOperations.Add(0xA25BFF81, new Operations(new Rol(3), new Xor(146), new Rol(4), new Add(88), new Ror(2), new Rol(3), new Add(4)));
            CryptOperations.Add(0xA3227799, new Operations(new Xor(195), new Rol(1), new Xor(254), new Ror(1), new Ror(1), new Rol(2), new Sub(6), new Ror(1)));
            CryptOperations.Add(0xA458D8B1, new Operations(new Xor(62), new Add(122), new Rol(3), new Rol(2), new Ror(4), new Sub(103), new Rol(4)));
            CryptOperations.Add(0xA4B3AB17, new Operations(new Xor(193), new Add(48), new Xor(226)));
            CryptOperations.Add(0xA53E24EE, new Operations(new Xor(134), new Sub(29), new Xor(55), new Dec(), new Rol(3), new Rol(4)));
            CryptOperations.Add(0xA78AEF49, new Operations(new Xor(250), new Sub(78), new Ror(4)));
            CryptOperations.Add(0xA8E9B9B9, new Operations(new Ror(1), new Ror(3), new Sub(99), new Xor(16), new Ror(1), new Ror(4)));
            CryptOperations.Add(0xAC48196C, new Operations(new Ror(2), new Ror(3), new Rol(4), new Sub(72), new Xor(136), new Rol(2), new Ror(3)));
            CryptOperations.Add(0xAC4A764, new Operations(new Add(54), new Ror(3), new Ror(2), new Sub(51), new Rol(2), new Add(5)));
            CryptOperations.Add(0xAE120565, new Operations(new Sub(71), new Ror(2), new Ror(1), new Sub(36), new Ror(3)));
            CryptOperations.Add(0xAEF46FE2, new Operations(new Xor(60), new Sub(16), new Ror(4), new Ror(4), new Rol(2), new Xor(110), new Rol(4)));
            CryptOperations.Add(0xAF0166D5, new Operations(new Add(57), new Rol(3), new Rol(2), new Ror(2), new Add(41), new Rol(1)));
            CryptOperations.Add(0xAFBC79ED, new Operations(new Sub(51), new Ror(2), new Add(119), new Rol(4), new Xor(9), new Add(96)));
            CryptOperations.Add(0xB04F87FB, new Operations(new Sub(5), new Ror(1), new Add(81), new Ror(3), new Rol(3), new Add(85), new Xor(178)));
            CryptOperations.Add(0xB1131842, new Operations(new Add(74), new Rol(3), new Sub(7), new Ror(3), new Ror(4), new Ror(2), new Rol(2)));
            CryptOperations.Add(0xB18E456, new Operations(new Sub(120), new Rol(2), new Add(86), new Rol(3), new Ror(2), new Rol(4), new Add(125)));
            CryptOperations.Add(0xB25306D1, new Operations(new Xor(48), new Add(29), new Xor(206), new Ror(2), new Rol(1)));
            CryptOperations.Add(0xB26A5566, new Operations(new Ror(2), new Ror(2), new Sub(87), new Ror(3), new Xor(130), new Sub(97), new Ror(2)));
            CryptOperations.Add(0xB29CD04F, new Operations(new Rol(1), new Rol(3), new Sub(90), new Rol(1), new Sub(112), new Rol(2), new Sub(99)));
            CryptOperations.Add(0xB31F2EF6, new Operations(new Xor(48), new Sub(98), new Xor(223), new Ror(1), new Add(25), new Rol(3)));
            CryptOperations.Add(0xB38E2C0F, new Operations(new Xor(225), new Ror(3), new Sub(25), new Rol(3)));
            CryptOperations.Add(0xB4533F87, new Operations(new Add(61), new Rol(3)));
            CryptOperations.Add(0xB47673FB, new Operations(new Rol(1), new Ror(2), new Xor(58), new Ror(3), new Ror(4), new Ror(2), new Rol(2), new Sub(108)));
            CryptOperations.Add(0xB48FE94E, new Operations(new Rol(3), new Xor(6), new Add(35), new Xor(57), new Sub(77), new Ror(4), new Sub(109)));
            CryptOperations.Add(0xB4D7D387, new Operations(new Xor(24), new Ror(2), new Xor(92), new Rol(3), new Add(46), new Rol(3), new Rol(1)));
            CryptOperations.Add(0xB4F36D5B, new Operations(new Ror(4), new Add(58), new Ror(1), new Xor(182), new Add(26), new Xor(58), new Ror(2)));
            CryptOperations.Add(0xB54E04E6, new Operations(new Xor(133), new Ror(2), new Add(106), new Xor(219), new Ror(3), new Sub(53)));
            CryptOperations.Add(0xB86024C5, new Operations(new Xor(44), new Rol(2), new Add(2), new Ror(1), new Rol(2), new Rol(1)));
            CryptOperations.Add(0xB8EA7FE9, new Operations(new Xor(164), new Ror(1), new Add(51)));
            CryptOperations.Add(0xB9EF0EF1, new Operations(new Ror(2), new Rol(1), new Rol(3), new Rol(2), new Ror(1), new Ror(4), new Sub(66)));
            CryptOperations.Add(0xBAD48E59, new Operations(new Sub(105), new Ror(3), new Sub(12), new Ror(3), new Xor(239), new Add(111), new Xor(59)));
            CryptOperations.Add(0xBAFD0E51, new Operations(new Rol(1), new Add(57), new Ror(4), new Sub(70)));
            CryptOperations.Add(0xBBC0884F, new Operations(new Ror(3), new Sub(75), new Rol(3), new Rol(2), new Xor(110)));
            CryptOperations.Add(0xBD567F7B, new Operations(new Add(103), new Xor(4), new Ror(2), new Add(43), new Rol(4), new Rol(3), new Sub(64)));
            CryptOperations.Add(0xBE609DE3, new Operations(new Xor(234), new Add(85), new Ror(1), new Rol(1), new Add(21), new Xor(18)));
            CryptOperations.Add(0xC0B58458, new Operations(new Add(18), new Xor(42), new Ror(3), new Sub(9), new Xor(116), new Sub(48), new Xor(74)));
            CryptOperations.Add(0xC20A701F, new Operations(new Xor(116), new Rol(4), new Ror(2), new Rol(3), new Add(29), new Rol(1), new Xor(178), new Ror(1)));
            CryptOperations.Add(0xC30D0126, new Operations(new Xor(96), new Ror(1), new Ror(4), new Ror(4), new Rol(3)));
            CryptOperations.Add(0xC34F6893, new Operations(new Ror(3), new Rol(3), new Ror(3), new Add(28), new Ror(1)));
            CryptOperations.Add(0xC3CD5DA4, new Operations(new Add(44), new Ror(2), new Add(120)));
            CryptOperations.Add(0xC5320C94, new Operations(new Ror(3), new Rol(3), new Ror(2), new Xor(140), new Inc(), new Xor(148)));
            CryptOperations.Add(0xC572E521, new Operations(new Ror(1), new Rol(2), new Sub(65), new Rol(3), new Xor(70)));
            CryptOperations.Add(0xC5B489A3, new Operations(new Xor(131), new Sub(90), new Xor(253), new Sub(23), new Ror(3)));
            CryptOperations.Add(0xC750FD6E, new Operations(new Sub(43), new Rol(1), new Add(44), new Xor(211), new Sub(69), new Xor(112)));
            CryptOperations.Add(0xC83043DB, new Operations(new Add(66), new Xor(174), new Rol(3), new Rol(3), new Ror(3), new Ror(4)));
            CryptOperations.Add(0xC8B77B5F, new Operations(new Xor(17), new Ror(1), new Sub(104), new Xor(89), new Sub(47), new Ror(4)));
            CryptOperations.Add(0xCAA8873F, new Operations(new Ror(1), new Xor(140), new Sub(9), new Xor(191)));
            CryptOperations.Add(0xCB51772A, new Operations(new Ror(3), new Rol(4), new Ror(3), new Ror(2), new Rol(1), new Rol(4), new Ror(2), new Rol(1)));
            CryptOperations.Add(0xCB8AF0AF, new Operations(new Rol(3), new Ror(1), new Ror(3), new Rol(1), new Add(92)));
            CryptOperations.Add(0xCB9CB67C, new Operations(new Sub(107), new Ror(3), new Add(110), new Xor(206), new Ror(1), new Xor(194)));
            CryptOperations.Add(0xCCDE2C27, new Operations(new Rol(1), new Rol(3), new Rol(1), new Rol(4), new Sub(54)));
            CryptOperations.Add(0xCCE043B8, new Operations(new Xor(182), new Add(83), new Xor(231), new Sub(24), new Rol(2), new Xor(96)));
            CryptOperations.Add(0xCE08CEA5, new Operations(new Sub(77), new Rol(4), new Xor(140)));
            CryptOperations.Add(0xCF8F05D9, new Operations(new Xor(237), new Rol(1), new Sub(53), new Rol(1), new Ror(3), new Add(72)));
            CryptOperations.Add(0xD13F6212, new Operations(new Ror(1), new Xor(118), new Sub(114), new Xor(124), new Add(19)));
            CryptOperations.Add(0xD330A8A7, new Operations(new Xor(235), new Rol(2), new Sub(118)));
            CryptOperations.Add(0xD38A1920, new Operations(new Xor(105), new Add(26), new Xor(242), new Add(89)));
            CryptOperations.Add(0xD3DF00B0, new Operations(new Xor(153), new Add(72), new Rol(3), new Sub(58), new Rol(2), new Xor(105)));
            CryptOperations.Add(0xD42DA55B, new Operations(new Xor(62), new Sub(14), new Ror(3), new Ror(1), new Add(15)));
            CryptOperations.Add(0xD4E6C9B1, new Operations(new Add(68), new Rol(2), new Sub(36), new Xor(88), new Add(4)));
            CryptOperations.Add(0xD55CA7B6, new Operations(new Sub(100), new Ror(3), new Add(43), new Rol(1), new Xor(51), new Rol(1)));
            CryptOperations.Add(0xD573497, new Operations(new Ror(1), new Sub(14), new Xor(17), new Ror(3), new Add(47)));
            CryptOperations.Add(0xD5D0E1EF, new Operations(new Ror(3), new Sub(117), new Xor(155)));
            CryptOperations.Add(0xD6519F4, new Operations(new Ror(4), new Ror(1), new Xor(147), new Add(43), new Xor(152)));
            CryptOperations.Add(0xD6E05E5E, new Operations(new Ror(1), new Xor(49), new Rol(3), new Xor(43), new Ror(3), new Xor(177)));
            CryptOperations.Add(0xD742D27E, new Operations(new Sub(88), new Rol(3), new Ror(2), new Ror(2), new Xor(76)));
            CryptOperations.Add(0xD7FE6D8F, new Operations(new Xor(133), new Sub(78), new Xor(89), new Sub(102)));
            CryptOperations.Add(0xD90633A7, new Operations(new Rol(3), new Add(4), new Xor(149), new Sub(79), new Rol(2), new Ror(2)));
            CryptOperations.Add(0xD972A251, new Operations(new Xor(39), new Add(17), new Ror(1), new Add(97), new Ror(1), new Ror(3)));
            CryptOperations.Add(0xD9CAA166, new Operations(new Ror(2), new Xor(186), new Rol(4), new Ror(4), new Rol(4), new Inc(), new Xor(172)));
            CryptOperations.Add(0xDB3282C3, new Operations(new Xor(96), new Rol(2), new Rol(3), new Rol(3), new Add(34), new Xor(101)));
            CryptOperations.Add(0xDBF6686B, new Operations(new Rol(4), new Xor(98), new Ror(2), new Xor(3), new Rol(2), new Rol(3), new Xor(120)));
            CryptOperations.Add(0xDCAB16A8, new Operations(new Add(120), new Rol(4), new Sub(11), new Rol(3), new Xor(198), new Sub(100)));
            CryptOperations.Add(0xDD13EC71, new Operations(new Xor(35), new Ror(1), new Rol(3), new Xor(237), new Ror(1), new Sub(111)));
            CryptOperations.Add(0xDDD2966E, new Operations(new Rol(4), new Ror(2), new Sub(22), new Xor(52), new Ror(2), new Ror(1), new Sub(81)));
            CryptOperations.Add(0xDE84029, new Operations(new Add(18), new Rol(2), new Xor(232), new Rol(4), new Ror(2)));
            CryptOperations.Add(0xDF588A63, new Operations(new Rol(3), new Rol(2), new Sub(70), new Rol(1), new Rol(2)));
            CryptOperations.Add(0xDF5BC7AB, new Operations(new Rol(1), new Rol(3), new Ror(2), new Xor(84), new Ror(1), new Add(92), new Xor(5)));
            CryptOperations.Add(0xE1C5F4F4, new Operations(new Ror(1), new Sub(34), new Ror(1), new Add(62), new Ror(1), new Rol(2), new Ror(3)));
            CryptOperations.Add(0xE2CF6727, new Operations(new Rol(2), new Xor(7), new Add(25), new Ror(4), new Sub(54), new Xor(163), new Ror(3)));
            CryptOperations.Add(0xE3DAF0C4, new Operations(new Rol(3), new Add(99), new Rol(3), new Xor(43), new Sub(4)));
            CryptOperations.Add(0xE6C93A07, new Operations(new Sub(114), new Rol(3), new Add(66), new Rol(2), new Rol(2), new Ror(3)));
            CryptOperations.Add(0xE7B14FA2, new Operations(new Xor(74), new Sub(72), new Rol(1), new Sub(66), new Rol(2), new Xor(244)));
            CryptOperations.Add(0xE86A8C8, new Operations(new Ror(2), new Xor(159), new Ror(2), new Xor(157), new Ror(1), new Xor(9)));
            CryptOperations.Add(0xE9558054, new Operations(new Add(106), new Ror(4), new Add(51), new Xor(72)));
            CryptOperations.Add(0xE9E0604E, new Operations(new Sub(69), new Xor(232)));
            CryptOperations.Add(0xEBB3429C, new Operations(new Ror(2), new Rol(2), new Sub(89), new Ror(2), new Ror(3), new Sub(31)));
            CryptOperations.Add(0xED38ECC8, new Operations(new Add(104), new Xor(194), new Add(119), new Ror(4), new Add(34)));
            CryptOperations.Add(0xF14F0ADF, new Operations(new Add(101), new Xor(96), new Rol(2), new Rol(1), new Rol(1), new Add(112), new Rol(3), new Xor(162)));
            CryptOperations.Add(0xF17B4DEA, new Operations(new Sub(9), new Ror(2), new Xor(216), new Ror(1), new Ror(2)));
            CryptOperations.Add(0xF35D2ED, new Operations(new Sub(94), new Xor(199), new Add(6), new Xor(4)));
            CryptOperations.Add(0xF64C7792, new Operations(new Rol(1), new Sub(52), new Xor(20), new Sub(7), new Xor(200)));
            CryptOperations.Add(0xF68409C9, new Operations(new Sub(92), new Rol(3), new Add(95), new Ror(1), new Sub(26), new Rol(3)));
            CryptOperations.Add(0xF7E8FB7F, new Operations(new Sub(102), new Rol(3), new Xor(211), new Add(12), new Ror(4), new Sub(38)));
            CryptOperations.Add(0xF8B85373, new Operations(new Ror(1), new Rol(1), new Add(64), new Rol(4), new Rol(3), new Sub(103), new Xor(205)));
            CryptOperations.Add(0xF959A10D, new Operations(new Sub(94), new Ror(3), new Ror(3), new Ror(3), new Add(13), new Xor(89), new Rol(4), new Xor(246)));
            CryptOperations.Add(0xFBD57D74, new Operations(new Ror(1), new Add(112), new Xor(251), new Rol(1), new Xor(24)));
            CryptOperations.Add(0xFD7AF57D, new Operations(new Rol(2), new Add(43), new Xor(8), new Rol(3), new Rol(3), new Ror(2), new Sub(83), new Rol(4)));
            CryptOperations.Add(0xFD7AF87C, new Operations(new Rol(1), new Sub(9), new Rol(2), new Xor(85), new Ror(2), new Rol(2), new Add(27)));
            CryptOperations.Add(0xFE0A65A2, new Operations(new Sub(103), new Ror(2), new Add(69), new Rol(3)));
            CryptOperations.Add(0xFF6A2DFB, new Operations(new Add(44), new Xor(170), new Ror(1), new Rol(2)));
            #endregion
        }

        public byte Encrypt(byte data)
        {
            foreach (var op in operations)
            {
                data = op.Encrypt(data);
            }
            return data;
        }

        public byte Decrypt(byte data)
        {
            foreach (var op in operations.Reverse())
            {
                data = op.Decrypt(data);
            }
            return data;
        }

        public static Operations GetOperations(uint hash)
        {
            return CryptOperations[hash];
        }
    }
}