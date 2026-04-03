
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.8.3928.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v2.0.50727
.assembly extern Activant.P21.Extensions
{
  .publickeytoken = (A3 19 98 6F D9 C8 53 C7 )                         // ...o..S.
  .ver 1:0:0:0
}
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern System.Windows.Forms
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern System.Data
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly KomarFrghtHandCharge56189
{
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 19 4B 6F 6D 61 72 46 72 67 68 74 48 61 6E   // ...KomarFrghtHan
                                                                                              64 43 68 61 72 67 65 35 36 31 38 39 00 00 )       // dCharge56189..
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 12 41 63 74 69 76 61 6E 74 20 53 6F 6C 75   // ...Activant Solu
                                                                                                74 69 6F 6E 73 00 00 )                            // tions..
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 19 4B 6F 6D 61 72 46 72 67 68 74 48 61 6E   // ...KomarFrghtHan
                                                                                                64 43 68 61 72 67 65 35 36 31 38 39 00 00 )       // dCharge56189..
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 24 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ..$Copyright .. 
                                                                                                  41 63 74 69 76 61 6E 74 20 53 6F 6C 75 74 69 6F   // Activant Solutio
                                                                                                  6E 73 20 32 30 31 33 00 00 )                      // ns 2013..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 63 34 66 38 36 30 31 32 2D 63 33 65 66   // ..$c4f86012-c3ef
                                                                                                  2D 34 66 30 30 2D 61 30 33 38 2D 33 35 36 61 39   // -4f00-a038-356a9
                                                                                                  38 31 34 34 65 39 62 00 00 )                      // 8144e9b..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom instance void [mscorlib]System.Security.AllowPartiallyTrustedCallersAttribute::.ctor() = ( 01 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 02 00 00 00 00 00 ) 

  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module KomarFrghtHandCharge56189.dll
// MVID: {713F8276-94E1-4FE5-AD66-9DDCB1E5344F}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00000126F6420000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit KomarFrghtHandCharge56189.KomarFrghtHandCharge56189
       extends [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule
{
  .field private string sRMAFlag
  .field private valuetype [mscorlib]System.Decimal dSalesSubTotal
  .field private valuetype [mscorlib]System.Decimal dNewFreightOut
  .field private valuetype [mscorlib]System.Decimal dFreightOut
  .field private string sFreightOut
  .field private string sNewFreightOut
  .field private string sRoute
  .field private string sSalesSubTotal
  .field private string sConn
  .field private string sRequireHandlingCharge
  .field private string sShipToID
  .field private string sShipToExempt
  .field private string sOeHdrExempt
  .field private bool bDebugMode
  .method public hidebysig virtual instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       952 (0x3b8)
    .maxstack  6
    .locals init (class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult V_0,
             class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields V_1,
             string[] V_2)
    IL_0000:  newobj     instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  ldc.i4.1
    IL_0008:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
    IL_000d:  ldarg.0
    IL_000e:  call       instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0013:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_0018:  stloc.1
    IL_0019:  ldarg.0
    IL_001a:  ldc.i4.5
    IL_001b:  newarr     [mscorlib]System.String
    IL_0020:  stloc.2
    IL_0021:  ldloc.2
    IL_0022:  ldc.i4.0
    IL_0023:  ldstr      "provider=sqloledb;server="
    IL_0028:  stelem.ref
    IL_0029:  ldloc.2
    IL_002a:  ldc.i4.1
    IL_002b:  ldloc.1
    IL_002c:  ldstr      "global_server"
    IL_0031:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0036:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_003b:  stelem.ref
    IL_003c:  ldloc.2
    IL_003d:  ldc.i4.2
    IL_003e:  ldstr      ";database="
    IL_0043:  stelem.ref
    IL_0044:  ldloc.2
    IL_0045:  ldc.i4.3
    IL_0046:  ldloc.1
    IL_0047:  ldstr      "global_database"
    IL_004c:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0051:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0056:  stelem.ref
    IL_0057:  ldloc.2
    IL_0058:  ldc.i4.4
    IL_0059:  ldstr      ";Integrated Security=SSPI"
    IL_005e:  stelem.ref
    IL_005f:  ldloc.2
    IL_0060:  call       string [mscorlib]System.String::Concat(string[])
    IL_0065:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sConn
    IL_006a:  ldarg.0
    IL_006b:  ldloc.1
    IL_006c:  ldstr      "rma_flag"
    IL_0071:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0076:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_007b:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRMAFlag
    IL_0080:  ldarg.0
    IL_0081:  ldloc.1
    IL_0082:  ldstr      "exempt_delivery_charge"
    IL_0087:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_008c:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0091:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_0096:  ldarg.0
    IL_0097:  ldloc.1
    IL_0098:  ldstr      "ship_to_id"
    IL_009d:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_00a2:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_00a7:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_00ac:  ldarg.0
    IL_00ad:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRMAFlag
    IL_00b2:  ldstr      "Y"
    IL_00b7:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_00bc:  brfalse    IL_03a3

    IL_00c1:  ldarg.0
    IL_00c2:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_00c7:  ldstr      "Y"
    IL_00cc:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_00d1:  brfalse    IL_038e

    IL_00d6:  ldarg.0
    IL_00d7:  ldarg.0
    IL_00d8:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_00dd:  ldarg.0
    IL_00de:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sConn
    IL_00e3:  ldloc.0
    IL_00e4:  call       instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::CheckExempt(string,
                                                                                                                                                                                  string,
                                                                                                                                                                                  class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult)
    IL_00e9:  pop
    IL_00ea:  ldarg.0
    IL_00eb:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
    IL_00f0:  ldstr      "Y"
    IL_00f5:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                     string)
    IL_00fa:  brfalse    IL_0379

    IL_00ff:  ldarg.0
    IL_0100:  ldloc.1
    IL_0101:  ldstr      "ship_route"
    IL_0106:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_010b:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_0110:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_0115:  ldarg.0
    IL_0116:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_011b:  brfalse.s  IL_0133

    IL_011d:  ldstr      "sRoute: "
    IL_0122:  ldarg.0
    IL_0123:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_0128:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_012d:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0132:  pop
    IL_0133:  ldarg.0
    IL_0134:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_0139:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
    IL_013e:  brtrue     IL_03b6

    IL_0143:  ldarg.0
    IL_0144:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0149:  brfalse.s  IL_0156

    IL_014b:  ldstr      "About to check route."
    IL_0150:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0155:  pop
    IL_0156:  ldarg.0
    IL_0157:  ldarg.0
    IL_0158:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRoute
    IL_015d:  ldarg.0
    IL_015e:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sConn
    IL_0163:  ldloc.0
    IL_0164:  call       instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::CheckRoute(string,
                                                                                                                                                                                 string,
                                                                                                                                                                                 class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult)
    IL_0169:  pop
    IL_016a:  ldarg.0
    IL_016b:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
    IL_0170:  ldstr      "Y"
    IL_0175:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_017a:  brfalse    IL_03b6

    IL_017f:  ldarg.0
    IL_0180:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0185:  brfalse.s  IL_0192

    IL_0187:  ldstr      "We will now check and see if the freight should be"
    + " applied and then allow the save"
    IL_018c:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0191:  pop
    IL_0192:  ldarg.0
    IL_0193:  ldloc.1
    IL_0194:  ldstr      "sales_sub_total"
    IL_0199:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_019e:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_01a3:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_01a8:  ldarg.0
    IL_01a9:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_01ae:  brfalse.s  IL_01c6

    IL_01b0:  ldstr      "sSalesSubTotal: "
    IL_01b5:  ldarg.0
    IL_01b6:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_01bb:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_01c0:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_01c5:  pop
    IL_01c6:  ldarg.0
    IL_01c7:  ldarg.0
    IL_01c8:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sSalesSubTotal
    IL_01cd:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_01d2:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_01d7:  ldarg.0
    IL_01d8:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_01dd:  brfalse.s  IL_01fa

    IL_01df:  ldstr      "dSalesSubTotal: "
    IL_01e4:  ldarg.0
    IL_01e5:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_01ea:  box        [mscorlib]System.Decimal
    IL_01ef:  call       string [mscorlib]System.String::Concat(object,
                                                                object)
    IL_01f4:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_01f9:  pop
    IL_01fa:  ldarg.0
    IL_01fb:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0200:  brfalse.s  IL_020d

    IL_0202:  ldstr      "About to check subtotal."
    IL_0207:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_020c:  pop
    IL_020d:  ldarg.0
    IL_020e:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_0213:  ldc.i4     0x61a8
    IL_0218:  ldc.i4.0
    IL_0219:  ldc.i4.0
    IL_021a:  ldc.i4.0
    IL_021b:  ldc.i4.2
    IL_021c:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_0221:  call       bool [mscorlib]System.Decimal::op_LessThan(valuetype [mscorlib]System.Decimal,
                                                                    valuetype [mscorlib]System.Decimal)
    IL_0226:  brfalse    IL_034e

    IL_022b:  ldarg.0
    IL_022c:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dSalesSubTotal
    IL_0231:  ldc.i4.0
    IL_0232:  ldc.i4.0
    IL_0233:  ldc.i4.0
    IL_0234:  ldc.i4.0
    IL_0235:  ldc.i4.2
    IL_0236:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_023b:  call       bool [mscorlib]System.Decimal::op_GreaterThan(valuetype [mscorlib]System.Decimal,
                                                                       valuetype [mscorlib]System.Decimal)
    IL_0240:  brfalse    IL_034e

    IL_0245:  ldarg.0
    IL_0246:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_024b:  brfalse.s  IL_0258

    IL_024d:  ldstr      "Less than 250.00 and greater than 0.00."
    IL_0252:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0257:  pop
    IL_0258:  ldarg.0
    IL_0259:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_025e:  brfalse.s  IL_026b

    IL_0260:  ldstr      "Charge required."
    IL_0265:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_026a:  pop
    IL_026b:  ldarg.0
    IL_026c:  ldloc.1
    IL_026d:  ldstr      "freight_out"
    IL_0272:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0277:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_027c:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_0281:  ldarg.0
    IL_0282:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0287:  brfalse.s  IL_029f

    IL_0289:  ldstr      "sFreightOut: "
    IL_028e:  ldarg.0
    IL_028f:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_0294:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_0299:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_029e:  pop
    IL_029f:  ldarg.0
    IL_02a0:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_02a5:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
    IL_02aa:  brtrue.s   IL_02bd

    IL_02ac:  ldarg.0
    IL_02ad:  ldarg.0
    IL_02ae:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sFreightOut
    IL_02b3:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_02b8:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dFreightOut
    IL_02bd:  ldarg.0
    IL_02be:  ldc.i4     0x5dc
    IL_02c3:  ldc.i4.0
    IL_02c4:  ldc.i4.0
    IL_02c5:  ldc.i4.0
    IL_02c6:  ldc.i4.2
    IL_02c7:  newobj     instance void [mscorlib]System.Decimal::.ctor(int32,
                                                                       int32,
                                                                       int32,
                                                                       bool,
                                                                       uint8)
    IL_02cc:  stfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02d1:  ldarg.0
    IL_02d2:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_02d7:  brfalse.s  IL_02f4

    IL_02d9:  ldstr      "dNewFreightOut: "
    IL_02de:  ldarg.0
    IL_02df:  ldfld      valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02e4:  box        [mscorlib]System.Decimal
    IL_02e9:  call       string [mscorlib]System.String::Concat(object,
                                                                object)
    IL_02ee:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_02f3:  pop
    IL_02f4:  ldarg.0
    IL_02f5:  ldarg.0
    IL_02f6:  ldflda     valuetype [mscorlib]System.Decimal KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::dNewFreightOut
    IL_02fb:  call       instance string [mscorlib]System.Decimal::ToString()
    IL_0300:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0305:  ldarg.0
    IL_0306:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_030b:  brfalse.s  IL_0323

    IL_030d:  ldstr      "sNewFreightOut: "
    IL_0312:  ldarg.0
    IL_0313:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0318:  call       string [mscorlib]System.String::Concat(string,
                                                                string)
    IL_031d:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0322:  pop
    IL_0323:  ldarg.0
    IL_0324:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0329:  brfalse.s  IL_0336

    IL_032b:  ldstr      "Set the new freight out."
    IL_0330:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0335:  pop
    IL_0336:  ldloc.1
    IL_0337:  ldstr      "freight_out"
    IL_033c:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_0341:  ldarg.0
    IL_0342:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0347:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_034c:  br.s       IL_03b6

    IL_034e:  ldarg.0
    IL_034f:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0354:  brfalse.s  IL_0361

    IL_0356:  ldstr      "About to zero out the freight."
    IL_035b:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_0360:  pop
    IL_0361:  ldloc.1
    IL_0362:  ldstr      "freight_out"
    IL_0367:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
    IL_036c:  ldarg.0
    IL_036d:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sNewFreightOut
    IL_0372:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_0377:  br.s       IL_03b6

    IL_0379:  ldarg.0
    IL_037a:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_037f:  brfalse.s  IL_03b6

    IL_0381:  ldstr      "We have an exempt ship to so we didn't do anything."
    IL_0386:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_038b:  pop
    IL_038c:  br.s       IL_03b6

    IL_038e:  ldarg.0
    IL_038f:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_0394:  brfalse.s  IL_03b6

    IL_0396:  ldstr      "We have an exempt order so we didn't do anything."
    IL_039b:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_03a0:  pop
    IL_03a1:  br.s       IL_03b6

    IL_03a3:  ldarg.0
    IL_03a4:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
    IL_03a9:  brfalse.s  IL_03b6

    IL_03ab:  ldstr      "We have an RMA so we didn't do anything."
    IL_03b0:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
    IL_03b5:  pop
    IL_03b6:  ldloc.0
    IL_03b7:  ret
  } // end of method KomarFrghtHandCharge56189::Execute

  .method public hidebysig instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult 
          CheckRoute(string sRoute,
                     string sConn,
                     class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult result) cil managed
  {
    // Code size       212 (0xd4)
    .maxstack  3
    .locals init (string V_0,
             class [System.Data]System.Data.OleDb.OleDbConnection V_1,
             class [System.Data]System.Data.OleDb.OleDbCommand V_2,
             class [System.Data]System.Data.OleDb.OleDbDataReader V_3,
             class [System.Data]System.Data.OleDb.OleDbException V_4,
             class [mscorlib]System.Exception V_5)
    IL_0000:  ldstr      "select shipping_route.route_code, shipping_route_u"
    + "d.require_handling_charge from shipping_route left join shipping_route_"
    + "ud on shipping_route.shipping_route_uid = shipping_route_ud.shipping_ro"
    + "ute_uid where shipping_route.route_code = '"
    IL_0005:  ldarg.1
    IL_0006:  ldstr      "' "
    IL_000b:  call       string [mscorlib]System.String::Concat(string,
                                                                string,
                                                                string)
    IL_0010:  stloc.0
    .try
    {
      IL_0011:  ldarg.2
      IL_0012:  newobj     instance void [System.Data]System.Data.OleDb.OleDbConnection::.ctor(string)
      IL_0017:  stloc.1
      IL_0018:  ldloc.0
      IL_0019:  ldloc.1
      IL_001a:  newobj     instance void [System.Data]System.Data.OleDb.OleDbCommand::.ctor(string,
                                                                                            class [System.Data]System.Data.OleDb.OleDbConnection)
      IL_001f:  stloc.2
      .try
      {
        IL_0020:  ldloc.1
        IL_0021:  callvirt   instance void [System.Data]System.Data.Common.DbConnection::Open()
        IL_0026:  ldloc.2
        IL_0027:  callvirt   instance class [System.Data]System.Data.OleDb.OleDbDataReader [System.Data]System.Data.OleDb.OleDbCommand::ExecuteReader()
        IL_002c:  stloc.3
        IL_002d:  ldloc.3
        IL_002e:  callvirt   instance bool [System.Data]System.Data.Common.DbDataReader::get_HasRows()
        IL_0033:  brfalse.s  IL_007f

        IL_0035:  ldarg.0
        IL_0036:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_003b:  brfalse.s  IL_0048

        IL_003d:  ldstr      "We have a route."
        IL_0042:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_0047:  pop
        IL_0048:  ldloc.3
        IL_0049:  callvirt   instance bool [System.Data]System.Data.Common.DbDataReader::Read()
        IL_004e:  pop
        IL_004f:  ldarg.0
        IL_0050:  ldloc.3
        IL_0051:  ldstr      "require_handling_charge"
        IL_0056:  callvirt   instance object [System.Data]System.Data.Common.DbDataReader::get_Item(string)
        IL_005b:  callvirt   instance string [mscorlib]System.Object::ToString()
        IL_0060:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_0065:  ldarg.0
        IL_0066:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_006b:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
        IL_0070:  brfalse.s  IL_0092

        IL_0072:  ldarg.0
        IL_0073:  ldstr      "N"
        IL_0078:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
        IL_007d:  br.s       IL_0092

        IL_007f:  ldarg.0
        IL_0080:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_0085:  brfalse.s  IL_0092

        IL_0087:  ldstr      "We do NOT have a route, this is a problem."
        IL_008c:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_0091:  pop
        IL_0092:  ldloc.3
        IL_0093:  callvirt   instance void [System.Data]System.Data.Common.DbDataReader::Close()
        IL_0098:  ldloc.1
        IL_0099:  callvirt   instance void [System.Data]System.Data.Common.DbConnection::Close()
        IL_009e:  leave.s    IL_00b8

      }  // end .try
      catch [System.Data]System.Data.OleDb.OleDbException 
      {
        IL_00a0:  stloc.s    V_4
        IL_00a2:  ldarg.3
        IL_00a3:  ldc.i4.0
        IL_00a4:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
        IL_00a9:  ldarg.3
        IL_00aa:  ldloc.s    V_4
        IL_00ac:  callvirt   instance string [mscorlib]System.Exception::get_Message()
        IL_00b1:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Message(string)
        IL_00b6:  leave.s    IL_00b8

      }  // end handler
      IL_00b8:  leave.s    IL_00d2

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_00ba:  stloc.s    V_5
      IL_00bc:  ldarg.3
      IL_00bd:  ldc.i4.0
      IL_00be:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_00c3:  ldarg.3
      IL_00c4:  ldloc.s    V_5
      IL_00c6:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_00cb:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_00d0:  leave.s    IL_00d2

    }  // end handler
    IL_00d2:  ldarg.3
    IL_00d3:  ret
  } // end of method KomarFrghtHandCharge56189::CheckRoute

  .method public hidebysig instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult 
          CheckExempt(string sShipToID,
                      string sConn,
                      class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult result) cil managed
  {
    // Code size       223 (0xdf)
    .maxstack  3
    .locals init (string V_0,
             class [System.Data]System.Data.OleDb.OleDbConnection V_1,
             class [System.Data]System.Data.OleDb.OleDbCommand V_2,
             class [System.Data]System.Data.OleDb.OleDbDataReader V_3,
             class [System.Data]System.Data.OleDb.OleDbException V_4,
             class [mscorlib]System.Exception V_5)
    IL_0000:  ldstr      "select exempt_delivery_charges from ship_to_ud whe"
    + "re ship_to_id = '"
    IL_0005:  ldarg.1
    IL_0006:  ldstr      "'"
    IL_000b:  call       string [mscorlib]System.String::Concat(string,
                                                                string,
                                                                string)
    IL_0010:  stloc.0
    .try
    {
      IL_0011:  ldarg.2
      IL_0012:  newobj     instance void [System.Data]System.Data.OleDb.OleDbConnection::.ctor(string)
      IL_0017:  stloc.1
      IL_0018:  ldloc.0
      IL_0019:  ldloc.1
      IL_001a:  newobj     instance void [System.Data]System.Data.OleDb.OleDbCommand::.ctor(string,
                                                                                            class [System.Data]System.Data.OleDb.OleDbConnection)
      IL_001f:  stloc.2
      .try
      {
        IL_0020:  ldloc.1
        IL_0021:  callvirt   instance void [System.Data]System.Data.Common.DbConnection::Open()
        IL_0026:  ldloc.2
        IL_0027:  callvirt   instance class [System.Data]System.Data.OleDb.OleDbDataReader [System.Data]System.Data.OleDb.OleDbCommand::ExecuteReader()
        IL_002c:  stloc.3
        IL_002d:  ldloc.3
        IL_002e:  callvirt   instance bool [System.Data]System.Data.Common.DbDataReader::get_HasRows()
        IL_0033:  brfalse.s  IL_007f

        IL_0035:  ldarg.0
        IL_0036:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_003b:  brfalse.s  IL_0048

        IL_003d:  ldstr      "We have a ship_to_ud."
        IL_0042:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_0047:  pop
        IL_0048:  ldloc.3
        IL_0049:  callvirt   instance bool [System.Data]System.Data.Common.DbDataReader::Read()
        IL_004e:  pop
        IL_004f:  ldarg.0
        IL_0050:  ldloc.3
        IL_0051:  ldstr      "exempt_delivery_charges"
        IL_0056:  callvirt   instance object [System.Data]System.Data.Common.DbDataReader::get_Item(string)
        IL_005b:  callvirt   instance string [mscorlib]System.Object::ToString()
        IL_0060:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_0065:  ldarg.0
        IL_0066:  ldfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_006b:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
        IL_0070:  brfalse.s  IL_009d

        IL_0072:  ldarg.0
        IL_0073:  ldstr      "N"
        IL_0078:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_007d:  br.s       IL_009d

        IL_007f:  ldarg.0
        IL_0080:  ldfld      bool KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::bDebugMode
        IL_0085:  brfalse.s  IL_0092

        IL_0087:  ldstr      "We do NOT have a ship_to_ud, this is okay."
        IL_008c:  call       valuetype [System.Windows.Forms]System.Windows.Forms.DialogResult [System.Windows.Forms]System.Windows.Forms.MessageBox::Show(string)
        IL_0091:  pop
        IL_0092:  ldarg.0
        IL_0093:  ldstr      "N"
        IL_0098:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
        IL_009d:  ldloc.3
        IL_009e:  callvirt   instance void [System.Data]System.Data.Common.DbDataReader::Close()
        IL_00a3:  ldloc.1
        IL_00a4:  callvirt   instance void [System.Data]System.Data.Common.DbConnection::Close()
        IL_00a9:  leave.s    IL_00c3

      }  // end .try
      catch [System.Data]System.Data.OleDb.OleDbException 
      {
        IL_00ab:  stloc.s    V_4
        IL_00ad:  ldarg.3
        IL_00ae:  ldc.i4.0
        IL_00af:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
        IL_00b4:  ldarg.3
        IL_00b5:  ldloc.s    V_4
        IL_00b7:  callvirt   instance string [mscorlib]System.Exception::get_Message()
        IL_00bc:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Message(string)
        IL_00c1:  leave.s    IL_00c3

      }  // end handler
      IL_00c3:  leave.s    IL_00dd

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_00c5:  stloc.s    V_5
      IL_00c7:  ldarg.3
      IL_00c8:  ldc.i4.0
      IL_00c9:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_00ce:  ldarg.3
      IL_00cf:  ldloc.s    V_5
      IL_00d1:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_00d6:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_00db:  leave.s    IL_00dd

    }  // end handler
    IL_00dd:  ldarg.3
    IL_00de:  ret
  } // end of method KomarFrghtHandCharge56189::CheckExempt

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  ldstr      "KomarFrghtHandCharge56189 - Sets freight handling "
    + "charge as required"
    IL_0005:  ret
  } // end of method KomarFrghtHandCharge56189::GetDescription

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  ldstr      "KomarFrghtHandCharge56189"
    IL_0005:  ret
  } // end of method KomarFrghtHandCharge56189::GetName

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       165 (0xa5)
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
    IL_006d:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sConn
    IL_0072:  ldarg.0
    IL_0073:  ldstr      ""
    IL_0078:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sRequireHandlingCharge
    IL_007d:  ldarg.0
    IL_007e:  ldstr      ""
    IL_0083:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToID
    IL_0088:  ldarg.0
    IL_0089:  ldstr      ""
    IL_008e:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sShipToExempt
    IL_0093:  ldarg.0
    IL_0094:  ldstr      ""
    IL_0099:  stfld      string KomarFrghtHandCharge56189.KomarFrghtHandCharge56189::sOeHdrExempt
    IL_009e:  ldarg.0
    IL_009f:  call       instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule::.ctor()
    IL_00a4:  ret
  } // end of method KomarFrghtHandCharge56189::.ctor

} // end of class KomarFrghtHandCharge56189.KomarFrghtHandCharge56189


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\DanShao\.vscode\p21_business_rules\.ildasm\KomarFrghtHandCharge56189.res
