using Mineshafts;
using System.Reflection;
using System.Runtime.InteropServices;

// Obecné informace o sestavení se řídí přes následující 
// sadu atributů. Změnou hodnot těchto atributů se upraví informace
// přidružené k sestavení.
[assembly: AssemblyTitle(Main.MODNAME)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct(Main.MODNAME)]
[assembly: AssemblyCopyright("Copyright ©  2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Nastavení ComVisible na false způsobí neviditelnost typů v tomto sestavení
// pro komponenty modelu COM. Pokud potřebujete přístup k typu v tomto sestavení
// modelu COM, nastavte atribut ComVisible daného typu na hodnotu True.
[assembly: ComVisible(false)]

// Následující GUID se používá pro ID knihovny typů, pokud je tento projekt vystavený pro COM.
[assembly: Guid("cbcbad3a-0d05-4433-8614-90b8d82d865f")]

// Informace o verzi sestavení se skládá z těchto čtyř hodnot:
//
//      Hlavní verze
//      Podverze
//      Číslo sestavení
//      Revize
//
// Můžete zadat všechny hodnoty nebo nastavit výchozí číslo buildu a revize
// pomocí zástupného znaku * takto:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Main.VERSION)]
[assembly: AssemblyFileVersion(Main.VERSION)]
