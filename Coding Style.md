# Coding Style (Recommended)
>Some of my old codes are not on this style.
- Constants: PascalCase
```csharp
const int ExampleValue = 123;
```
- Properties: PascalCase
```csharp
int ExampleProperty { get; set; }
```
- Public Methods: PascalCase
```csharp
public void DoSomething() {}
```
- Private Methods : camelCase
```csharp
private void doSomething() {}
```
- Local Variables: camelCase
```csharp
public void DoSomething(int firstArgument,string secondArgument)
{
    firstArgument++;
    decimal localValue = 123;
    Console.WriteLine(secondArgument);
}
```
- Place left brace on new line (Shown above)
- Always use braces and new line in an if statement
```csharp
if (someCondition)
{
    doSomething();
}
else
{
    doSomethingElse();
}
```
- If you don't think a class will have derived class, seal it.
```csharp
sealed class MyClass
{
    /*...*/
}
```