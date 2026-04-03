
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.8.3928.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern P21.Extensions
{
  .publickeytoken = (0C D9 A9 72 A8 B8 C3 84 )                         // ...r....
  .ver 2:0:0:0
}
.assembly KomarPriceRounding_v2
{
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 15 4B 6F 6D 61 72 50 72 69 63 65 52 6F 75   // ...KomarPriceRou
                                                                                              6E 64 69 6E 67 5F 76 32 00 00 )                   // nding_v2..
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 15 4B 6F 6D 61 72 50 72 69 63 65 52 6F 75   // ...KomarPriceRou
                                                                                                6E 64 69 6E 67 5F 76 32 00 00 )                   // nding_v2..
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                  20 32 30 32 35 00 00 )                            //  2025..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 66 65 31 36 36 36 37 33 2D 66 63 36 66   // ..$fe166673-fc6f
                                                                                                  2D 34 66 36 30 2D 39 31 35 64 2D 39 32 32 39 62   // -4f60-915d-9229b
                                                                                                  36 37 38 64 36 37 32 00 00 )                      // 678d672..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom instance void [mscorlib]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = ( 01 00 1C 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                        2C 56 65 72 73 69 6F 6E 3D 76 34 2E 37 2E 32 01   // ,Version=v4.7.2.
                                                                                                        00 54 0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73   // .T..FrameworkDis
                                                                                                        70 6C 61 79 4E 61 6D 65 14 2E 4E 45 54 20 46 72   // playName..NET Fr
                                                                                                        61 6D 65 77 6F 72 6B 20 34 2E 37 2E 32 )          // amework 4.7.2
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module KomarPriceRounding_v2.dll
// MVID: {247AA8D8-4BAF-4656-A942-6C99D37670D4}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x0000018BBE2C0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit KomarPriceRounding_v2.KomarPriceRounding_v2
       extends [P21.Extensions]P21.Extensions.BusinessRule.Rule
{
  .method public hidebysig virtual instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       116 (0x74)
    .maxstack  2
    .locals init (class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_0,
             string V_1,
             bool V_2,
             class [mscorlib]System.Exception V_3,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_4)
    IL_0000:  nop
    IL_0001:  newobj     instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0006:  stloc.0
    .try
    {
      IL_0007:  nop
      IL_0008:  ldarg.0
      IL_0009:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_000e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0013:  ldstr      "rma_flag"
      IL_0018:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_001d:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldstr      "Y"
      IL_0029:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                       string)
      IL_002e:  stloc.2
      IL_002f:  ldloc.2
      IL_0030:  brfalse.s  IL_0045

      IL_0032:  nop
      IL_0033:  ldarg.0
      IL_0034:  call       instance void KomarPriceRounding_v2.KomarPriceRounding_v2::roundPrice()
      IL_0039:  nop
      IL_003a:  ldloc.0
      IL_003b:  ldc.i4.1
      IL_003c:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_0041:  nop
      IL_0042:  nop
      IL_0043:  br.s       IL_004f

      IL_0045:  nop
      IL_0046:  ldloc.0
      IL_0047:  ldc.i4.1
      IL_0048:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_004d:  nop
      IL_004e:  nop
      IL_004f:  nop
      IL_0050:  leave.s    IL_006c

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_0052:  stloc.3
      IL_0053:  nop
      IL_0054:  ldloc.0
      IL_0055:  ldc.i4.0
      IL_0056:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_005b:  nop
      IL_005c:  ldloc.0
      IL_005d:  ldloc.3
      IL_005e:  callvirt   instance string [mscorlib]System.Object::ToString()
      IL_0063:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0068:  nop
      IL_0069:  nop
      IL_006a:  leave.s    IL_006c

    }  // end handler
    IL_006c:  ldloc.0
    IL_006d:  stloc.s    V_4
    IL_006f:  br.s       IL_0071

    IL_0071:  ldloc.s    V_4
    IL_0073:  ret
  } // end of method KomarPriceRounding_v2::Execute

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "Round price base on system calc unit price."
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarPriceRounding_v2::GetDescription

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "KomarPriceRounding_v2"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarPriceRounding_v2::GetName

  .method public hidebysig instance void 
          roundPrice() cil managed
  {
    // Code size       211 (0xd3)
    .maxstack  2
    .locals init (string V_0,
             valuetype [mscorlib]System.Decimal V_1,
             string V_2,
             valuetype [mscorlib]System.Decimal V_3,
             valuetype [mscorlib]System.Decimal V_4,
             string V_5,
             bool V_6,
             bool V_7)
    IL_0000:  nop
    IL_0001:  ldarg.0
    IL_0002:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0007:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_000c:  ldstr      "unit_price"
    IL_0011:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0016:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_001b:  stloc.0
    IL_001c:  ldloc.0
    IL_001d:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_0022:  stloc.1
    IL_0023:  ldarg.0
    IL_0024:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0029:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_002e:  ldstr      "system_calc_unit_price"
    IL_0033:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0038:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
    IL_003d:  stloc.2
    IL_003e:  ldloc.2
    IL_003f:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Convert::ToDecimal(string)
    IL_0044:  stloc.3
    IL_0045:  ldloc.0
    IL_0046:  ldstr      "0.000000001"
    IL_004b:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0050:  stloc.s    V_6
    IL_0052:  ldloc.s    V_6
    IL_0054:  brfalse.s  IL_00cf

    IL_0056:  nop
    IL_0057:  ldloc.3
    IL_0058:  ldsfld     valuetype [mscorlib]System.Decimal [mscorlib]System.Decimal::One
    IL_005d:  call       bool [mscorlib]System.Decimal::op_LessThan(valuetype [mscorlib]System.Decimal,
                                                                    valuetype [mscorlib]System.Decimal)
    IL_0062:  stloc.s    V_7
    IL_0064:  ldloc.s    V_7
    IL_0066:  brfalse.s  IL_009b

    IL_0068:  nop
    IL_0069:  ldloc.3
    IL_006a:  ldc.i4.4
    IL_006b:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Math::Round(valuetype [mscorlib]System.Decimal,
                                                                                         int32)
    IL_0070:  stloc.s    V_4
    IL_0072:  ldloc.s    V_4
    IL_0074:  call       string [mscorlib]System.Convert::ToString(valuetype [mscorlib]System.Decimal)
    IL_0079:  stloc.s    V_5
    IL_007b:  ldarg.0
    IL_007c:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0081:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_0086:  ldstr      "unit_price"
    IL_008b:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0090:  ldloc.s    V_5
    IL_0092:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_0097:  nop
    IL_0098:  nop
    IL_0099:  br.s       IL_00cc

    IL_009b:  nop
    IL_009c:  ldloc.3
    IL_009d:  ldc.i4.2
    IL_009e:  call       valuetype [mscorlib]System.Decimal [mscorlib]System.Math::Round(valuetype [mscorlib]System.Decimal,
                                                                                         int32)
    IL_00a3:  stloc.s    V_4
    IL_00a5:  ldloc.s    V_4
    IL_00a7:  call       string [mscorlib]System.Convert::ToString(valuetype [mscorlib]System.Decimal)
    IL_00ac:  stloc.s    V_5
    IL_00ae:  ldarg.0
    IL_00af:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_00b4:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_00b9:  ldstr      "unit_price"
    IL_00be:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_00c3:  ldloc.s    V_5
    IL_00c5:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_00ca:  nop
    IL_00cb:  nop
    IL_00cc:  nop
    IL_00cd:  br.s       IL_00d2

    IL_00cf:  nop
    IL_00d0:  br.s       IL_00d2

    IL_00d2:  ret
  } // end of method KomarPriceRounding_v2::roundPrice

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [P21.Extensions]P21.Extensions.BusinessRule.Rule::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method KomarPriceRounding_v2::.ctor

} // end of class KomarPriceRounding_v2.KomarPriceRounding_v2

.class public auto ansi beforefieldinit KomarPriceRounding_v2.KomarPriceTrigger_v2
       extends [P21.Extensions]P21.Extensions.BusinessRule.Rule
{
  .method public hidebysig virtual instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       116 (0x74)
    .maxstack  2
    .locals init (class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_0,
             string V_1,
             bool V_2,
             class [mscorlib]System.Exception V_3,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_4)
    IL_0000:  nop
    IL_0001:  newobj     instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0006:  stloc.0
    .try
    {
      IL_0007:  nop
      IL_0008:  ldarg.0
      IL_0009:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_000e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0013:  ldstr      "rma_flag"
      IL_0018:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_001d:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_0022:  stloc.1
      IL_0023:  ldloc.1
      IL_0024:  ldstr      "Y"
      IL_0029:  call       bool [mscorlib]System.String::op_Inequality(string,
                                                                       string)
      IL_002e:  stloc.2
      IL_002f:  ldloc.2
      IL_0030:  brfalse.s  IL_0045

      IL_0032:  nop
      IL_0033:  ldarg.0
      IL_0034:  call       instance void KomarPriceRounding_v2.KomarPriceTrigger_v2::triggerPrice()
      IL_0039:  nop
      IL_003a:  ldloc.0
      IL_003b:  ldc.i4.1
      IL_003c:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_0041:  nop
      IL_0042:  nop
      IL_0043:  br.s       IL_004f

      IL_0045:  nop
      IL_0046:  ldloc.0
      IL_0047:  ldc.i4.1
      IL_0048:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_004d:  nop
      IL_004e:  nop
      IL_004f:  nop
      IL_0050:  leave.s    IL_006c

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_0052:  stloc.3
      IL_0053:  nop
      IL_0054:  ldloc.0
      IL_0055:  ldc.i4.0
      IL_0056:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_005b:  nop
      IL_005c:  ldloc.0
      IL_005d:  ldloc.3
      IL_005e:  callvirt   instance string [mscorlib]System.Object::ToString()
      IL_0063:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0068:  nop
      IL_0069:  nop
      IL_006a:  leave.s    IL_006c

    }  // end handler
    IL_006c:  ldloc.0
    IL_006d:  stloc.s    V_4
    IL_006f:  br.s       IL_0071

    IL_0071:  ldloc.s    V_4
    IL_0073:  ret
  } // end of method KomarPriceTrigger_v2::Execute

  .method public hidebysig instance void 
          triggerPrice() cil managed
  {
    // Code size       34 (0x22)
    .maxstack  8
    IL_0000:  nop
    IL_0001:  ldarg.0
    IL_0002:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
    IL_0007:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
    IL_000c:  ldstr      "unit_price"
    IL_0011:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
    IL_0016:  ldstr      "0.000000001"
    IL_001b:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
    IL_0020:  nop
    IL_0021:  ret
  } // end of method KomarPriceTrigger_v2::triggerPrice

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "Trigger a price change."
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarPriceTrigger_v2::GetDescription

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "KomarPriceTrigger_v2"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method KomarPriceTrigger_v2::GetName

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [P21.Extensions]P21.Extensions.BusinessRule.Rule::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method KomarPriceTrigger_v2::.ctor

} // end of class KomarPriceRounding_v2.KomarPriceTrigger_v2


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\DanShao\.vscode\p21_business_rules\.ildasm\KomarPriceRounding_v2.res
