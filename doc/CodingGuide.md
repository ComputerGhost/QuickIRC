
# Coding Guidelines

This document contains Nathan Belue's guidelines for coding in Visual Basic .Net. This document version is 1.0.0.

The goal of this document is to reduce cognitive load by providing simple tips for making coding decisions. If used by more than one programmer, a coding guide also helps to ensure code consistency. However, these are not rules, and a programmer may deviate from the guidelines at their discretion.


# Organization

## Folders and Files

It should be easy to find a file that handles a specific part of the code. While the perfect folder structure varies depending on project type and size, there are some common guidelines to achieve this goal:

 * Folders should never be nested more than three levels deep.
 * Files should contain exactly one data type definition.
 * Files should be named upon their content.

## File Contents

Files and their contents should be easy to grok at a glance.

Do not include useless information such as a license or excessive documentation. This should be in a separate file, if possible.

Organize the code well, provide comments where needed, and reduce scrolling.

## Classes and Modules

A consistent organization helps in finding code that does a certain task. Definitions should be sorted by visibility, type, then name.

Visibility should be sorted as follows:
 * Public
 * Protected
 * Friend
 * Private

Types should be organized as follows:
 * Events and Delegates
 * Properties and private class variables
 * Shared methods
 * New, Dispose, and related methods
 * Other methods


# Naming

Names should be descriptive. Do not use abbreviations unless the abbreviation is common; for example, using `i` as an iterator is okay (though not recommended).

## Files

Files should be named the same as the data type that they contain, which will almost always be in upper camel case.

## Data Types

Data types like classes, enumerations, and structures should have a name in upper camel case. This includes custom control names.

Data type examples: ChatRoom, MessageEnumerator, SyntaxError

## Interfaces

Interface names should be prefixed with an "I". The rest of the name should be in upper camel case.

Interface examples: IIterator, IChatroom, ILockable

## Methods

Subprocedure and function names should be in upper camel case. The first word should be a verb.

The exception to this is event handlers, which are the property name followed by an underscore and the event name.

Method examples: ClearMessages, IsEmpty, Release, btnSend_Click

## Properties

Property and private variable names should be descriptive and in upper camel case.

One exception is auto-generated code that is not used outside of the designer. Do not waste time renaming these.

The other exception is user input, which should be designated as such by using Hungarian notation. This is to indicate that the value may need to be sanitized. Prefixes are based upon the type of value:

 * btn: Button
 * chk: Checkbox
 * dt: Date
 * lst: List
 * txt: TextBox

Property examples: Count, Messages, UserModes, Label1, btnSend


## Local Variables

Local variable names should be in lowercase, with each word separated by an underscore.

Local variable examples: current_char, saved_offset, transaction


# Formatting

## Tab Stops

Tab stops should be 4 characters apart. Spaces should be used instead of tabs.

## Line Width

Most lines should not exceed 78 characters. This is to reduce the need for scrolling. With that goal in mind, do not meet this requirement by adding to the vertical scrolling distance. It is okay for less important text to extend beyond the margin if needed.

## Comments

A public interface, such as a class or subprocedure, may have comments that describe how to use it. Internal code may have comments that describe how it works.


```
    ' Assumes that message is syntactically valid
    Sub HandleNickReceived(message As Message)
        Dim new_nick = message.Parameters(0)

        ' Keep track of our own nickname
        If Connection.Nickname = message.Source.Name Then
            Connection.Nickname = new_nick
            Connection.UserHost = message.Source.Raw
        End If

        ' Rename in DOS protection lists
        If IgnoredNicks.ContainsKey(sender) Then
            IgnoredNicks.Add(new_nick, IgnoredNicks(sender))
            IgnoredNicks.Remove(sender)
        End If
        If WatchedNicks.ContainsKey(sender) Then
            WatchedNicks.Add(new_nick, WatchedNicks(new_nick))
            WatchedNicks.Remove(sender)
        End If

    End Sub
```


# Code

## Advanced Features

Advanced features are okay if they actually make the code better. Do not use "cool" features just to show off.

## Try...Catch and Exceptions

In a perfect world, exceptions should never happen. They should only be thrown when the program is in an unexpected state.

Exceptions should only be caught where they will be handled. Do not use Try-Catch blocks to ignore errors.

