
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
.assembly OE_CreditCard
{
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 0D 4F 45 5F 43 72 65 64 69 74 43 61 72 64   // ...OE_CreditCard
                                                                                              00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 0D 4F 45 5F 43 72 65 64 69 74 43 61 72 64   // ...OE_CreditCard
                                                                                                00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                  20 32 30 32 35 00 00 )                            //  2025..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 36 31 35 37 39 37 65 36 2D 64 65 34 66   // ..$615797e6-de4f
                                                                                                  2D 34 36 35 32 2D 62 64 36 61 2D 32 30 32 35 31   // -4652-bd6a-20251
                                                                                                  33 37 38 37 35 32 30 00 00 )                      // 3787520..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom instance void [mscorlib]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = ( 01 00 1C 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                        2C 56 65 72 73 69 6F 6E 3D 76 34 2E 37 2E 32 01   // ,Version=v4.7.2.
                                                                                                        00 54 0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73   // .T..FrameworkDis
                                                                                                        70 6C 61 79 4E 61 6D 65 14 2E 4E 45 54 20 46 72   // playName..NET Fr
                                                                                                        61 6D 65 77 6F 72 6B 20 34 2E 37 2E 32 )          // amework 4.7.2
  .hash algorithm 0x00008004
  .ver 1:0:9428:15582
}
.module OE_CreditCard.dll
// MVID: {D328C32A-4E3B-44E5-9B0C-FFD82C3AC803}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x0000020D1C7A0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit OE_CreditCard.OE_CreditCard
       extends [P21.Extensions]P21.Extensions.BusinessRule.Rule
{
  .method public hidebysig virtual instance class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       386 (0x182)
    .maxstack  4
    .locals init (class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_0,
             string V_1,
             string V_2,
             bool V_3,
             string V_4,
             string V_5,
             string V_6,
             string V_7,
             bool V_8,
             bool V_9,
             bool V_10,
             bool V_11,
             class [P21.Extensions]P21.Extensions.BusinessRule.RuleResult V_12,
             class [mscorlib]System.Exception V_13)
    IL_0000:  nop
    IL_0001:  newobj     instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0006:  dup
    IL_0007:  ldc.i4.1
    IL_0008:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
    IL_000d:  nop
    IL_000e:  stloc.0
    .try
    {
      IL_000f:  nop
      IL_0010:  ldarg.0
      IL_0011:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_0016:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_001b:  ldstr      "oe_hdr_terms"
      IL_0020:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_0025:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_002a:  stloc.1
      IL_002b:  ldarg.0
      IL_002c:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_0031:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0036:  ldstr      "approved"
      IL_003b:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_0040:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_0045:  stloc.2
      IL_0046:  ldloc.1
      IL_0047:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
      IL_004c:  brtrue.s   IL_0069

      IL_004e:  ldloc.1
      IL_004f:  ldstr      "Credit Card"
      IL_0054:  ldc.i4.5
      IL_0055:  callvirt   instance bool [mscorlib]System.String::Equals(string,
                                                                         valuetype [mscorlib]System.StringComparison)
      IL_005a:  brfalse.s  IL_0069

      IL_005c:  ldloc.2
      IL_005d:  ldstr      "Y"
      IL_0062:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0067:  br.s       IL_006a

      IL_0069:  ldc.i4.0
      IL_006a:  stloc.3
      IL_006b:  ldloc.3
      IL_006c:  brfalse    IL_015b

      IL_0071:  nop
      IL_0072:  ldarg.0
      IL_0073:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_0078:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_007d:  ldstr      "cc_name"
      IL_0082:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_0087:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_008c:  stloc.s    V_4
      IL_008e:  ldarg.0
      IL_008f:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_0094:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0099:  ldstr      "cc_creditcard_number"
      IL_009e:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_00a3:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_00a8:  stloc.s    V_5
      IL_00aa:  ldarg.0
      IL_00ab:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_00b0:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_00b5:  ldstr      "cc_expiration_date"
      IL_00ba:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_00bf:  callvirt   instance string [P21.Extensions]P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_00c4:  stloc.s    V_6
      IL_00c6:  ldnull
      IL_00c7:  stloc.s    V_7
      IL_00c9:  ldloc.s    V_4
      IL_00cb:  call       bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
      IL_00d0:  stloc.s    V_8
      IL_00d2:  ldloc.s    V_8
      IL_00d4:  brfalse.s  IL_00df

      IL_00d6:  ldstr      "credit card name"
      IL_00db:  stloc.s    V_7
      IL_00dd:  br.s       IL_0109

      IL_00df:  ldloc.s    V_5
      IL_00e1:  call       bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
      IL_00e6:  stloc.s    V_9
      IL_00e8:  ldloc.s    V_9
      IL_00ea:  brfalse.s  IL_00f5

      IL_00ec:  ldstr      "credit card number"
      IL_00f1:  stloc.s    V_7
      IL_00f3:  br.s       IL_0109

      IL_00f5:  ldloc.s    V_6
      IL_00f7:  call       bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
      IL_00fc:  stloc.s    V_10
      IL_00fe:  ldloc.s    V_10
      IL_0100:  brfalse.s  IL_0109

      IL_0102:  ldstr      "credit card expiration date"
      IL_0107:  stloc.s    V_7
      IL_0109:  ldloc.s    V_7
      IL_010b:  ldnull
      IL_010c:  cgt.un
      IL_010e:  stloc.s    V_11
      IL_0110:  ldloc.s    V_11
      IL_0112:  brfalse.s  IL_015a

      IL_0114:  nop
      IL_0115:  ldarg.0
      IL_0116:  call       instance class [P21.Extensions]P21.Extensions.BusinessRule.DataCollection [P21.Extensions]P21.Extensions.BusinessRule.Rule::get_Data()
      IL_011b:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataFields [P21.Extensions]P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0120:  ldstr      "approved"
      IL_0125:  callvirt   instance class [P21.Extensions]P21.Extensions.BusinessRule.DataField [P21.Extensions]P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_012a:  ldstr      "N"
      IL_012f:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
      IL_0134:  nop
      IL_0135:  ldloc.0
      IL_0136:  ldc.i4.0
      IL_0137:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_013c:  nop
      IL_013d:  ldloc.0
      IL_013e:  ldstr      "No "
      IL_0143:  ldloc.s    V_7
      IL_0145:  ldstr      " has been specified. Order must remain Unapproved "
      + "until all credit card details are entered."
      IL_014a:  call       string [mscorlib]System.String::Concat(string,
                                                                  string,
                                                                  string)
      IL_014f:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0154:  nop
      IL_0155:  ldloc.0
      IL_0156:  stloc.s    V_12
      IL_0158:  leave.s    IL_017f

      IL_015a:  nop
      IL_015b:  nop
      IL_015c:  leave.s    IL_017a

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_015e:  stloc.s    V_13
      IL_0160:  nop
      IL_0161:  ldloc.0
      IL_0162:  ldc.i4.0
      IL_0163:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_0168:  nop
      IL_0169:  ldloc.0
      IL_016a:  ldloc.s    V_13
      IL_016c:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_0171:  callvirt   instance void [P21.Extensions]P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0176:  nop
      IL_0177:  nop
      IL_0178:  leave.s    IL_017a

    }  // end handler
    IL_017a:  ldloc.0
    IL_017b:  stloc.s    V_12
    IL_017d:  br.s       IL_017f

    IL_017f:  ldloc.s    V_12
    IL_0181:  ret
  } // end of method OE_CreditCard::Execute

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "Validate Credit Card details for Orders with Credi"
    + "t Card Terms."
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method OE_CreditCard::GetDescription

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "OE_CreditCard"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method OE_CreditCard::GetName

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [P21.Extensions]P21.Extensions.BusinessRule.Rule::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  } // end of method OE_CreditCard::.ctor

} // end of class OE_CreditCard.OE_CreditCard


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\DanShao\.vscode\p21_business_rules\.ildasm\OE_CreditCard.res
