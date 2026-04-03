
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.8.3928.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern P21.Extensions
{
  .publickeytoken = (0C D9 A9 72 A8 B8 C3 84 )                         // ...r....
  .ver 2:0:0:0
}
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern System.Windows.Forms
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern System.Data
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly KomarFrghtHandCharge56189
{
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 19 4B 6F 6D 61 72 46 72 67 68 74 48 61 6E   // ...KomarFrghtHan
                                                                                              64 43 68 61 72 67 65 35 36 31 38 39 00 00 )       // dCharge56189..
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 56 52 65 62 75 69 6C 74 20 50 72 6F 70 68   // ..VRebuilt Proph
                                                                                                    65 74 20 32 31 20 66 72 65 69 67 68 74 20 68 61   // et 21 freight ha
                                                                                                    6E 64 6C 69 6E 67 20 63 68 61 72 67 65 20 72 75   // ndling charge ru
                                                                                                    6C 65 20 77 69 74 68 20 66 72 61 6D 65 77 6F 72   // le with framewor
                                                                                                    6B 2D 6D 61 6E 61 67 65 64 20 53 51 4C 20 63 6F   // k-managed SQL co
                                                                                                    6E 6E 65 63 74 69 6F 6E 2E 00 00 )                // nnection...
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 0E 4B 6F 6D 61 72 20 41 6C 6C 69 61 6E 63   // ...Komar Allianc
                                                                                                65 00 00 )                                        // e..
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 19 4B 6F 6D 61 72 46 72 67 68 74 48 61 6E   // ...KomarFrghtHan
                                                                                                64 43 68 61 72 67 65 35 36 31 38 39 00 00 )       // dCharge56189..
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 1B 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                  4B 6F 6D 61 72 20 41 6C 6C 69 61 6E 63 65 00 00 ) // Komar Alliance..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 46 31 36 43 33 44 36 2D 30 32 44 32   // ..$5F16C3D6-02D2
                                                                                                  2D 34 37 44 37 2D 39 38 44 37 2D 37 31 39 37 46   // -47D7-98D7-7197F
                                                                                                  39 43 37 33 32 46 34 00 00 )                      // 9C732F4..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module KomarFrghtHandCharge56189.dll
// MVID: {4D569F0E-369E-4EAB-8C17-DF99476B81F5}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x0000024E85700000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit KomarFrghtHandCharge56189.KomarFrghtHandCharge56189
       extends [P21.Extensions]P21.Extensions.BusinessRule.Rule
{
  .field private string sRMAFlag
  .field private valuetype [mscorlib]System.Decimal dSalesSubTotal
  .field private valuetype [mscorlib]System.Decimal dNewFreightOut
  .field private valuetype [mscorlib]System.Decimal dFreightOut
  .field private string sFreightOut
  .field private string sNewFreightOut
  .field private string sRoute
  .field private string sSalesSubTotal
  .field private string sRequireHandlingCharge
  .field private string sShipToID
  .field private string sShipToExempt
  .field private string sOeHdrExempt
  .field private bool bDebugMode
  .method public hidebysig virtual instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       980 (0x3d4)
    .maxstack  6
    .locals init (class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_0,
             class [P21.Extensions]P21.Extensions.BusinessRule.DataFields V_1,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_2,
             bool V_3)
    IL_0000:  nop
    IL_0001:  newobj     instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  ldc.i4.1
    IL_0009:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
    IL_000e:  nop
    IL_000f:  ldarg.0
    IL_0010:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0015:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_001a:  stloc.1
    IL_001b:  ldarg.0
    IL_001c:  ldloc.1
    IL_001d:  ldstr      "rma_flag"
    IL_0022:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0027:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_002c:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRMAFlag
    IL_0031:  ldarg.0
    IL_0032:  ldloc.1
    IL_0033:  ldstr      "exempt_delivery_charge"
    IL_0038:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_003d:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0042:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_0047:  ldarg.0
    IL_0048:  ldloc.1
    IL_0049:  ldstr      "ship_to_id"
    IL_004e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0053:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0058:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_005d:  ldarg.0
    IL_005e:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRMAFlag
    IL_0063:  ldstr      "Y"
    IL_0068:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_006d:  ldc.i4.0
    IL_006e:  ceq
    IL_0070:  stloc.3
    IL_0071:  ldloc.3
    IL_0072:  brtrue     IL_03b4

    IL_0077:  nop
    IL_0078:  ldarg.0
    IL_0079:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_007e:  ldstr      "Y"
    IL_0083:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_0088:  ldc.i4.0
    IL_0089:  ceq
    IL_008b:  stloc.3
    IL_008c:  ldloc.3
    IL_008d:  brtrue     IL_0397

    IL_0092:  nop
    IL_0093:  ldarg.0
    IL_0094:  ldarg.0
    IL_0095:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_009a:  ldloc.0
    IL_009b:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::CheckExempt(string,
                                                                                                                                                                class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult)
    IL_00a0:  pop
    IL_00a1:  ldarg.0
    IL_00a2:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
    IL_00a7:  ldstr      "Y"
    IL_00ac:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_00b1:  ldc.i4.0
    IL_00b2:  ceq
    IL_00b4:  stloc.3
    IL_00b5:  ldloc.3
    IL_00b6:  brtrue     IL_037a

    IL_00bb:  nop
    IL_00bc:  ldarg.0
    IL_00bd:  ldloc.1
    IL_00be:  ldstr      "ship_route"
    IL_00c3:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_00c8:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_00cd:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_00d2:  ldarg.0
    IL_00d3:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_00d8:  ldc.i4.0
    IL_00d9:  ceq
    IL_00db:  stloc.3
    IL_00dc:  ldloc.3
    IL_00dd:  brtrue.s   IL_00f7

    IL_00df:  nop
    IL_00e0:  ldstr      "sRoute: "
    IL_00e5:  ldarg.0
    IL_00e6:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_00eb:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_00f0:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_00f5:  pop
    IL_00f6:  nop
    IL_00f7:  ldarg.0
    IL_00f8:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_00fd:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
    IL_0102:  stloc.3
    IL_0103:  ldloc.3
    IL_0104:  brtrue     IL_0377

    IL_0109:  nop
    IL_010a:  ldarg.0
    IL_010b:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0110:  ldc.i4.0
    IL_0111:  ceq
    IL_0113:  stloc.3
    IL_0114:  ldloc.3
    IL_0115:  brtrue.s   IL_0124

    IL_0117:  nop
    IL_0118:  ldstr      "About to check route."
    IL_011d:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0122:  pop
    IL_0123:  nop
    IL_0124:  ldarg.0
    IL_0125:  ldarg.0
    IL_0126:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_012b:  ldloc.0
    IL_012c:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::CheckRoute(string,
                                                                                                                                                               class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult)
    IL_0131:  pop
    IL_0132:  ldarg.0
    IL_0133:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
    IL_0138:  ldstr      "Y"
    IL_013d:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0142:  ldc.i4.0
    IL_0143:  ceq
    IL_0145:  stloc.3
    IL_0146:  ldloc.3
    IL_0147:  brtrue     IL_0376

    IL_014c:  nop
    IL_014d:  ldarg.0
    IL_014e:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0153:  ldc.i4.0
    IL_0154:  ceq
    IL_0156:  stloc.3
    IL_0157:  ldloc.3
    IL_0158:  brtrue.s   IL_0167

    IL_015a:  nop
    IL_015b:  ldstr      "We will now check and see if the freight should be"
    + " applied and then allow the save"
    IL_0160:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0165:  pop
    IL_0166:  nop
    IL_0167:  ldarg.0
    IL_0168:  ldloc.1
    IL_0169:  ldstr      "sales_sub_total"
    IL_016e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0173:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0178:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_017d:  ldarg.0
    IL_017e:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0183:  ldc.i4.0
    IL_0184:  ceq
    IL_0186:  stloc.3
    IL_0187:  ldloc.3
    IL_0188:  brtrue.s   IL_01a2

    IL_018a:  nop
    IL_018b:  ldstr      "sSalesSubTotal: "
    IL_0190:  ldarg.0
    IL_0191:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_0196:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_019b:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_01a0:  pop
    IL_01a1:  nop
    IL_01a2:  ldarg.0
    IL_01a3:  ldarg.0
    IL_01a4:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_01a9:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_01ae:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_01b3:  ldarg.0
    IL_01b4:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_01b9:  ldc.i4.0
    IL_01ba:  ceq
    IL_01bc:  stloc.3
    IL_01bd:  ldloc.3
    IL_01be:  brtrue.s   IL_01e8

    IL_01c0:  nop
    IL_01c1:  ldstr      "dSalesSubTotal: "
    IL_01c6:  ldarg.0
    IL_01c7:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_01cc:  box        [mscorlib]System.Decimal
    IL_01d1:  call       string [mscorlib]System.String::Concat(object,
                                                                object)
    IL_01d6:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_01db:  pop
    IL_01dc:  ldstr      "About to check subtotal."
    IL_01e1:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_01e6:  pop
    IL_01e7:  nop
    IL_01e8:  ldarg.0
    IL_01e9:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_01ee:  ldc.i4     0x61a8
    IL_01f3:  ldc.i4.0
    IL_01f4:  ldc.i4.0
    IL_01f5:  ldc.i4.0
    IL_01f6:  ldc.i4.2
    IL_01f7:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_01fc:  call       bool [mscorlib]System.Decimal::op_LessThan(valuetype [mscorlib]System.Decimal,
                                                                    valuetype [mscorlib]System.Decimal)
    IL_0201:  brfalse.s  IL_021d

    IL_0203:  ldarg.0
    IL_0204:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_0209:  ldc.i4.0
    IL_020a:  ldc.i4.0
    IL_020b:  ldc.i4.0
    IL_020c:  ldc.i4.0
    IL_020d:  ldc.i4.2
    IL_020e:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_0213:  call       bool [mscorlib]System.Decimal::op_GreaterThan(valuetype [mscorlib]System.Decimal,
                                                                       valuetype [mscorlib]System.Decimal)
    IL_0218:  ldc.i4.0
    IL_0219:  ceq
    IL_021b:  br.s       IL_021e

    IL_021d:  ldc.i4.1
    IL_021e:  nop
    IL_021f:  stloc.3
    IL_0220:  ldloc.3
    IL_0221:  brtrue     IL_0342

    IL_0226:  nop
    IL_0227:  ldarg.0
    IL_0228:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_022d:  ldc.i4.0
    IL_022e:  ceq
    IL_0230:  stloc.3
    IL_0231:  ldloc.3
    IL_0232:  brtrue.s   IL_024c

    IL_0234:  nop
    IL_0235:  ldstr      "Less than 250.00 and greater than 0.00."
    IL_023a:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_023f:  pop
    IL_0240:  ldstr      "Charge required."
    IL_0245:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_024a:  pop
    IL_024b:  nop
    IL_024c:  ldarg.0
    IL_024d:  ldloc.1
    IL_024e:  ldstr      "freight_out"
    IL_0253:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0258:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_025d:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_0262:  ldarg.0
    IL_0263:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0268:  ldc.i4.0
    IL_0269:  ceq
    IL_026b:  stloc.3
    IL_026c:  ldloc.3
    IL_026d:  brtrue.s   IL_0287

    IL_026f:  nop
    IL_0270:  ldstr      "sFreightOut: "
    IL_0275:  ldarg.0
    IL_0276:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_027b:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_0280:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0285:  pop
    IL_0286:  nop
    IL_0287:  ldarg.0
    IL_0288:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_028d:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
    IL_0292:  stloc.3
    IL_0293:  ldloc.3
    IL_0294:  brtrue.s   IL_02a9

    IL_0296:  nop
    IL_0297:  ldarg.0
    IL_0298:  ldarg.0
    IL_0299:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_029e:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_02a3:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dFreightOut
    IL_02a8:  nop
    IL_02a9:  ldarg.0
    IL_02aa:  ldc.i4     0x5dc
    IL_02af:  ldc.i4.0
    IL_02b0:  ldc.i4.0
    IL_02b1:  ldc.i4.0
    IL_02b2:  ldc.i4.2
    IL_02b3:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_02b8:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02bd:  ldarg.0
    IL_02be:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_02c3:  ldc.i4.0
    IL_02c4:  ceq
    IL_02c6:  stloc.3
    IL_02c7:  ldloc.3
    IL_02c8:  brtrue.s   IL_02e7

    IL_02ca:  nop
    IL_02cb:  ldstr      "dNewFreightOut: "
    IL_02d0:  ldarg.0
    IL_02d1:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02d6:  box        [mscorlib]System.Decimal
    IL_02db:  call       string [mscorlib]System.String::Concat(object,
                                                                object)
    IL_02e0:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_02e5:  pop
    IL_02e6:  nop
    IL_02e7:  ldarg.0
    IL_02e8:  ldarg.0
    IL_02e9:  ldflda     valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02ee:  call       instance string [mscorlib]System.Decimal::ToString()
    IL_02f3:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_02f8:  ldarg.0
    IL_02f9:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_02fe:  ldc.i4.0
    IL_02ff:  ceq
    IL_0301:  stloc.3
    IL_0302:  ldloc.3
    IL_0303:  brtrue.s   IL_0328

    IL_0305:  nop
    IL_0306:  ldstr      "sNewFreightOut: "
    IL_030b:  ldarg.0
    IL_030c:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0311:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_0316:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_031b:  pop
    IL_031c:  ldstr      "Set the new freight out."
    IL_0321:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0326:  pop
    IL_0327:  nop
    IL_0328:  ldloc.1
    IL_0329:  ldstr      "freight_out"
    IL_032e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0333:  ldarg.0
    IL_0334:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0339:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_033e:  nop
    IL_033f:  nop
    IL_0340:  br.s       IL_0375

    IL_0342:  nop
    IL_0343:  ldarg.0
    IL_0344:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0349:  ldc.i4.0
    IL_034a:  ceq
    IL_034c:  stloc.3
    IL_034d:  ldloc.3
    IL_034e:  brtrue.s   IL_035d

    IL_0350:  nop
    IL_0351:  ldstr      "About to zero out the freight."
    IL_0356:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_035b:  pop
    IL_035c:  nop
    IL_035d:  ldloc.1
    IL_035e:  ldstr      "freight_out"
    IL_0363:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0368:  ldarg.0
    IL_0369:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_036e:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_0373:  nop
    IL_0374:  nop
    IL_0375:  nop
    IL_0376:  nop
    IL_0377:  nop
    IL_0378:  br.s       IL_0394

    IL_037a:  ldarg.0
    IL_037b:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0380:  ldc.i4.0
    IL_0381:  ceq
    IL_0383:  stloc.3
    IL_0384:  ldloc.3
    IL_0385:  brtrue.s   IL_0394

    IL_0387:  nop
    IL_0388:  ldstr      "We have an exempt ship to so we didn't do anything."
    IL_038d:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0392:  pop
    IL_0393:  nop
    IL_0394:  nop
    IL_0395:  br.s       IL_03b1

    IL_0397:  ldarg.0
    IL_0398:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_039d:  ldc.i4.0
    IL_039e:  ceq
    IL_03a0:  stloc.3
    IL_03a1:  ldloc.3
    IL_03a2:  brtrue.s   IL_03b1

    IL_03a4:  nop
    IL_03a5:  ldstr      "We have an exempt order so we didn't do anything."
    IL_03aa:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_03af:  pop
    IL_03b0:  nop
    IL_03b1:  nop
    IL_03b2:  br.s       IL_03ce

    IL_03b4:  ldarg.0
    IL_03b5:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_03ba:  ldc.i4.0
    IL_03bb:  ceq
    IL_03bd:  stloc.3
    IL_03be:  ldloc.3
    IL_03bf:  brtrue.s   IL_03ce

    IL_03c1:  nop
    IL_03c2:  ldstr      "We have an RMA so we didn't do anything."
    IL_03c7:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_03cc:  pop
    IL_03cd:  nop
    IL_03ce:  ldloc.0
    IL_03cf:  stloc.2
    IL_03d0:  br.s       IL_03d2

    IL_03d2:  ldloc.2
    IL_03d3:  ret
  } // end of method KomarFrghtHandCharge56189::Execute

  .method public hidebysig instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          CheckRoute(string sRouteValue,
                     class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult result) cil managed
  {
    // Code size       273 (0x111)
    .maxstack  3
    .locals init (class [System.Data]System.Data.SqlClient.SqlCommand V_0,
             object V_1,
             class [System.Data]System.Data.SqlClient.SqlException V_2,
             class [mscorlib]System.Exception V_3,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_4,
             bool V_5)
    IL_0000:  nop
    .try
    {
      IL_0001:  nop
      IL_0002:  ldstr      "\nselect shipping_route_ud.require_handling_charge\n"
      + "from shipping_route\nleft join shipping_route_ud\n    on shipping_route.s"
      + "hipping_route_uid = shipping_route_ud.shipping_route_uid\nwhere shipping"
      + "_route.route_code = @routeCode"
      IL_0007:  ldarg.0
      IL_0008:  call       instance class [System.Data]System.Data.SqlClient.SqlConnection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_P21SqlConnection()
      IL_000d:  newobj     instance void [System.Data]System.Data.SqlClient.SqlCommand::.ctor(string,
                                                                                              class [System.Data]System.Data.SqlClient.SqlConnection)
      IL_0012:  stloc.0
      .try
      {
        IL_0013:  nop
        IL_0014:  ldloc.0
        IL_0015:  callvirt   instance class [System.Data]System.Data.SqlClient.SqlParameterCollection [System.Data]System.Data.SqlClient.SqlCommand::get_Parameters()
        IL_001a:  ldstr      "@routeCode"
        IL_001f:  ldc.i4.s   22
        IL_0021:  callvirt   instance class [System.Data]System.Data.SqlClient.SqlParameter [System.Data]System.Data.SqlClient.SqlParameterCollection::Add(string,
                                                                                                                                                           valuetype [System.Data]System.Data.SqlDbType)
        IL_0026:  ldarg.1
        IL_0027:  dup
        IL_0028:  brtrue.s   IL_0030

        IL_002a:  pop
        IL_002b:  ldsfld     class [mscorlib]System.DBNull [mscorlib]System.DBNull::Value
        IL_0030:  callvirt   instance void [System.Data]System.Data.Common.DbParameter::set_Value(object)
        IL_0035:  nop
        IL_0036:  ldloc.0
        IL_0037:  callvirt   instance object [System.Data]System.Data.Common.DbCommand::ExecuteScalar()
        IL_003c:  stloc.1
        IL_003d:  ldloc.1
        IL_003e:  brfalse.s  IL_004a

        IL_0040:  ldloc.1
        IL_0041:  ldsfld     class [mscorlib]System.DBNull [mscorlib]System.DBNull::Value
        IL_0046:  ceq
        IL_0048:  br.s       IL_004b

        IL_004a:  ldc.i4.1
        IL_004b:  nop
        IL_004c:  stloc.s    V_5
        IL_004e:  ldloc.s    V_5
        IL_0050:  brtrue.s   IL_009f

        IL_0052:  nop
        IL_0053:  ldarg.0
        IL_0054:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_0059:  ldc.i4.0
        IL_005a:  ceq
        IL_005c:  stloc.s    V_5
        IL_005e:  ldloc.s    V_5
        IL_0060:  brtrue.s   IL_006f

        IL_0062:  nop
        IL_0063:  ldstr      "We have a route."
        IL_0068:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_006d:  pop
        IL_006e:  nop
        IL_006f:  ldarg.0
        IL_0070:  ldloc.1
        IL_0071:  call       string [mscorlib]System.Convert::ToString(object)
        IL_0076:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_007b:  ldarg.0
        IL_007c:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_0081:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
        IL_0086:  ldc.i4.0
        IL_0087:  ceq
        IL_0089:  stloc.s    V_5
        IL_008b:  ldloc.s    V_5
        IL_008d:  brtrue.s   IL_009c

        IL_008f:  nop
        IL_0090:  ldarg.0
        IL_0091:  ldstr      "N"
        IL_0096:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_009b:  nop
        IL_009c:  nop
        IL_009d:  br.s       IL_00bb

        IL_009f:  ldarg.0
        IL_00a0:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_00a5:  ldc.i4.0
        IL_00a6:  ceq
        IL_00a8:  stloc.s    V_5
        IL_00aa:  ldloc.s    V_5
        IL_00ac:  brtrue.s   IL_00bb

        IL_00ae:  nop
        IL_00af:  ldstr      "We do NOT have a route, this is a problem."
        IL_00b4:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_00b9:  pop
        IL_00ba:  nop
        IL_00bb:  nop
        IL_00bc:  leave.s    IL_00d0

      }  // end .try
      finally
      {
        IL_00be:  ldloc.0
        IL_00bf:  ldnull
        IL_00c0:  ceq
        IL_00c2:  stloc.s    V_5
        IL_00c4:  ldloc.s    V_5
        IL_00c6:  brtrue.s   IL_00cf

        IL_00c8:  ldloc.0
        IL_00c9:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
        IL_00ce:  nop
        IL_00cf:  endfinally
      }  // end handler
      IL_00d0:  nop
      IL_00d1:  nop
      IL_00d2:  leave.s    IL_0108

    }  // end .try
    catch [System.Data]System.Data.SqlClient.SqlException 
    {
      IL_00d4:  stloc.2
      IL_00d5:  nop
      IL_00d6:  ldarg.2
      IL_00d7:  ldc.i4.0
      IL_00d8:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_00dd:  nop
      IL_00de:  ldarg.2
      IL_00df:  ldloc.2
      IL_00e0:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_00e5:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_00ea:  nop
      IL_00eb:  nop
      IL_00ec:  leave.s    IL_0108

    }  // end handler
    catch [mscorlib]System.Exception 
    {
      IL_00ee:  stloc.3
      IL_00ef:  nop
      IL_00f0:  ldarg.2
      IL_00f1:  ldc.i4.0
      IL_00f2:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_00f7:  nop
      IL_00f8:  ldarg.2
      IL_00f9:  ldloc.3
      IL_00fa:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_00ff:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0104:  nop
      IL_0105:  nop
      IL_0106:  leave.s    IL_0108

    }  // end handler
    IL_0108:  nop
    IL_0109:  ldarg.2
    IL_010a:  stloc.s    V_4
    IL_010c:  br.s       IL_010e

    IL_010e:  ldloc.s    V_4
    IL_0110:  ret
  } // end of method KomarFrghtHandCharge56189::CheckRoute

  .method public hidebysig instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          CheckExempt(string sShipToIdValue,
                      class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult result) cil managed
  {
    // Code size       286 (0x11e)
    .maxstack  3
    .locals init (class [System.Data]System.Data.SqlClient.SqlCommand V_0,
             object V_1,
             class [System.Data]System.Data.SqlClient.SqlException V_2,
             class [mscorlib]System.Exception V_3,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_4,
             bool V_5)
    IL_0000:  nop
    .try
    {
      IL_0001:  nop
      IL_0002:  ldstr      "\nselect exempt_delivery_charges\nfrom ship_to_ud\nwh"
      + "ere ship_to_id = @shipToId"
      IL_0007:  ldarg.0
      IL_0008:  call       instance class [System.Data]System.Data.SqlClient.SqlConnection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_P21SqlConnection()
      IL_000d:  newobj     instance void [System.Data]System.Data.SqlClient.SqlCommand::.ctor(string,
                                                                                              class [System.Data]System.Data.SqlClient.SqlConnection)
      IL_0012:  stloc.0
      .try
      {
        IL_0013:  nop
        IL_0014:  ldloc.0
        IL_0015:  callvirt   instance class [System.Data]System.Data.SqlClient.SqlParameterCollection [System.Data]System.Data.SqlClient.SqlCommand::get_Parameters()
        IL_001a:  ldstr      "@shipToId"
        IL_001f:  ldc.i4.s   22
        IL_0021:  callvirt   instance class [System.Data]System.Data.SqlClient.SqlParameter [System.Data]System.Data.SqlClient.SqlParameterCollection::Add(string,
                                                                                                                                                           valuetype [System.Data]System.Data.SqlDbType)
        IL_0026:  ldarg.1
        IL_0027:  dup
        IL_0028:  brtrue.s   IL_0030

        IL_002a:  pop
        IL_002b:  ldsfld     class [mscorlib]System.DBNull [mscorlib]System.DBNull::Value
        IL_0030:  callvirt   instance void [System.Data]System.Data.Common.DbParameter::set_Value(object)
        IL_0035:  nop
        IL_0036:  ldloc.0
        IL_0037:  callvirt   instance object [System.Data]System.Data.Common.DbCommand::ExecuteScalar()
        IL_003c:  stloc.1
        IL_003d:  ldloc.1
        IL_003e:  brfalse.s  IL_004a

        IL_0040:  ldloc.1
        IL_0041:  ldsfld     class [mscorlib]System.DBNull [mscorlib]System.DBNull::Value
        IL_0046:  ceq
        IL_0048:  br.s       IL_004b

        IL_004a:  ldc.i4.1
        IL_004b:  nop
        IL_004c:  stloc.s    V_5
        IL_004e:  ldloc.s    V_5
        IL_0050:  brtrue.s   IL_009f

        IL_0052:  nop
        IL_0053:  ldarg.0
        IL_0054:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_0059:  ldc.i4.0
        IL_005a:  ceq
        IL_005c:  stloc.s    V_5
        IL_005e:  ldloc.s    V_5
        IL_0060:  brtrue.s   IL_006f

        IL_0062:  nop
        IL_0063:  ldstr      "We have a ship_to_ud."
        IL_0068:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_006d:  pop
        IL_006e:  nop
        IL_006f:  ldarg.0
        IL_0070:  ldloc.1
        IL_0071:  call       string [mscorlib]System.Convert::ToString(object)
        IL_0076:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_007b:  ldarg.0
        IL_007c:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_0081:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
        IL_0086:  ldc.i4.0
        IL_0087:  ceq
        IL_0089:  stloc.s    V_5
        IL_008b:  ldloc.s    V_5
        IL_008d:  brtrue.s   IL_009c

        IL_008f:  nop
        IL_0090:  ldarg.0
        IL_0091:  ldstr      "N"
        IL_0096:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_009b:  nop
        IL_009c:  nop
        IL_009d:  br.s       IL_00c8

        IL_009f:  nop
        IL_00a0:  ldarg.0
        IL_00a1:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_00a6:  ldc.i4.0
        IL_00a7:  ceq
        IL_00a9:  stloc.s    V_5
        IL_00ab:  ldloc.s    V_5
        IL_00ad:  brtrue.s   IL_00bc

        IL_00af:  nop
        IL_00b0:  ldstr      "We do NOT have a ship_to_ud, this is okay."
        IL_00b5:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_00ba:  pop
        IL_00bb:  nop
        IL_00bc:  ldarg.0
        IL_00bd:  ldstr      "N"
        IL_00c2:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_00c7:  nop
        IL_00c8:  nop
        IL_00c9:  leave.s    IL_00dd

      }  // end .try
      finally
      {
        IL_00cb:  ldloc.0
        IL_00cc:  ldnull
        IL_00cd:  ceq
        IL_00cf:  stloc.s    V_5
        IL_00d1:  ldloc.s    V_5
        IL_00d3:  brtrue.s   IL_00dc

        IL_00d5:  ldloc.0
        IL_00d6:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
        IL_00db:  nop
        IL_00dc:  endfinally
      }  // end handler
      IL_00dd:  nop
      IL_00de:  nop
      IL_00df:  leave.s    IL_0115

    }  // end .try
    catch [System.Data]System.Data.SqlClient.SqlException 
    {
      IL_00e1:  stloc.2
      IL_00e2:  nop
      IL_00e3:  ldarg.2
      IL_00e4:  ldc.i4.0
      IL_00e5:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_00ea:  nop
      IL_00eb:  ldarg.2
      IL_00ec:  ldloc.2
      IL_00ed:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_00f2:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_00f7:  nop
      IL_00f8:  nop
      IL_00f9:  leave.s    IL_0115

    }  // end handler
    catch [mscorlib]System.Exception 
    {
      IL_00fb:  stloc.3
      IL_00fc:  nop
      IL_00fd:  ldarg.2
      IL_00fe:  ldc.i4.0
      IL_00ff:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_0104:  nop
      IL_0105:  ldarg.2
      IL_0106:  ldloc.3
      IL_0107:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_010c:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0111:  nop
      IL_0112:  nop
      IL_0113:  leave.s    IL_0115

    }  // end handler
    IL_0115:  nop
    IL_0116:  ldarg.2
    IL_0117:  stloc.s    V_4
    IL_0119:  br.s       IL_011b

    IL_011b:  ldloc.s    V_4
    IL_011d:  ret
  } // end of method KomarFrghtHandCharge56189::CheckExempt

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "KomarFrghtHandCharge56189 - Sets freight handling "
    + "charge as required"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarFrghtHandCharge56189::GetDescription

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "KomarFrghtHandCharge56189"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarFrghtHandCharge56189::GetName

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       155 (0x9b)
    .maxstack  6
    IL_0000:  ldarg.0
    IL_0001:  ldstr      ""
    IL_0006:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRMAFlag
    IL_000b:  ldarg.0
    IL_000c:  ldc.i4.0
    IL_000d:  ldc.i4.0
    IL_000e:  ldc.i4.0
    IL_000f:  ldc.i4.0
    IL_0010:  ldc.i4.1
    IL_0011:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_0016:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_001b:  ldarg.0
    IL_001c:  ldc.i4.0
    IL_001d:  ldc.i4.0
    IL_001e:  ldc.i4.0
    IL_001f:  ldc.i4.0
    IL_0020:  ldc.i4.1
    IL_0021:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_0026:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_002b:  ldarg.0
    IL_002c:  ldc.i4.0
    IL_002d:  ldc.i4.0
    IL_002e:  ldc.i4.0
    IL_002f:  ldc.i4.0
    IL_0030:  ldc.i4.1
    IL_0031:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_0036:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dFreightOut
    IL_003b:  ldarg.0
    IL_003c:  ldstr      ""
    IL_0041:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_0046:  ldarg.0
    IL_0047:  ldstr      ""
    IL_004c:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0051:  ldarg.0
    IL_0052:  ldstr      ""
    IL_0057:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_005c:  ldarg.0
    IL_005d:  ldstr      ""
    IL_0062:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_0067:  ldarg.0
    IL_0068:  ldstr      ""
    IL_006d:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
    IL_0072:  ldarg.0
    IL_0073:  ldstr      ""
    IL_0078:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_007d:  ldarg.0
    IL_007e:  ldstr      ""
    IL_0083:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
    IL_0088:  ldarg.0
    IL_0089:  ldstr      ""
    IL_008e:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_0093:  ldarg.0
    IL_0094:  call       instance void [P21.Extensions]P21.Extensions.BusinessRule.Rule::.ctor()
    IL_0099:  nop
    IL_009a:  ret
  } // end of method KomarFrghtHandCharge56189::.ctor

} // end of class KomarFrghtHandCharge56189.KomarFrghtHandCharge56189


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\DanShao\.vscode\p21_business_rules\.ildasm\KomarFrghtHandCharge56189_new.res
