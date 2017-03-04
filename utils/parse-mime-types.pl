#!/usr/bin/perl
use strict;
use warnings;
use File::Fetch;
use Fcntl qw(:DEFAULT :flock);

#----------------------------------------------------------------------------------#
# Run from the command line. This will first parse the Apache mime.types file from #
# here (http://svn.apache.org/repos/asf/httpd/httpd/trunk/docs/conf/mime.types),   #
# and then the dictionary portion only of this (http://stackoverflow.com/a/7161265)#
# StackOverflow answer (starting on the fifth line down), saved as mime.dict. All  #
# files are expected to be in the same directory.                                  #
#                                                                                  #
# When parsing the dictionary, if an entry is found that was already found in the  #
# Apache mime.types file, you will be prompted to make a selection to use the new  #
# value from the dictionary (1) or the old value from mime.types (2), or you can   #
# provide your own value instead.                                                  #
#                                                                                  #
# Types are binary by default, and are changed to text if they meet any of the     #
# following three conditions:                                                      #
#                                                                                  #
# 1. Does the type begin with the word "text"?                                     #
# 2. Does the type end in "+xml" or "xml"?                                         #
# 3. Does the type contain the word "json" or "javascript"                         #
#                                                                                  #
# Extensions that start with digits or contain a dash (-) are no longer ignored.   #
#----------------------------------------------------------------------------------#

my (%extensions, $subtotala, $subtotals, $identical, $conflicts, $grandtotal);

my $inputs = "./mime.dict";
my $output = "./content-types.txt";

# Remove any previous output file
unlink($output) if (-e $output);

#----------------------------------------------------------------------------------#
# MIMEType Mismatch Resolver                                                       #
#----------------------------------------------------------------------------------#
sub resolve ($$$)
{
    my ($ext, $new, $old) = @_;

    print "\nExtension Mismatch : $ext\n";
    print "(1) New : <$new>\n";
    print "(2) Old : <$old>\n";
    print "Which one would you like to use?\n";
    print "<enter> or 1 for New, 2 for Old, or type your own > ";

    my $value = <STDIN>;
    chop($value);
    $value = 1 unless ($value);

    return ($value eq "1") ? $new : ($value eq "2") ? $old : $value;
}
#----------------------------------------------------------------------------------#


#----------------------------------------------------------------------------------#
# Parse Apache Extensions                                                          #
#----------------------------------------------------------------------------------#
{
    my $url = 'http://svn.apache.org/repos/asf/httpd/httpd/trunk/docs/conf/mime.types';
    my $ff = File::Fetch->new(uri => $url);
    my $file = $ff->fetch() or die $ff->error;

    open(INPUTA, $file);
    my @inputa = <INPUTA>;
    close(INPUTA);

    foreach my $line (@inputa)
    {
        next if ($line =~ /^#/);
        next if (length($line) < 1);
        $line =~ s/\r?\n//;

        my ($type, $extensions) = split(/\s+/, $line, 2);
        next unless ($extensions);

        foreach my $extension (split(/\s+/, $extensions))
        {
            $extensions{uc($extension)} = $type;
        }
    }

    $subtotala = (scalar (keys %extensions));
    unlink($file);
}
#----------------------------------------------------------------------------------#


#----------------------------------------------------------------------------------#
# Download StackOverflow Extensions                                                #
#----------------------------------------------------------------------------------#
{
    my $url = 'http://stackoverflow.com/a/7161265';

    my $ff = File::Fetch->new(uri => $url);
    my $file = $ff->fetch() or die $ff->error;

    my $addtodict = 0;

    open(OUTPUT, ">$inputs");
    flock (OUTPUT, LOCK_EX);

    open(INPUT, $file);
    flock (INPUT, LOCK_SH);
    while (my $line = <INPUT>)
    {
        last if ($addtodict && $line =~ /\s+};/);
        $addtodict ++ if ($addtodict || $line =~ /<pre><code>public static class MIMEAssistant/);
        print OUTPUT "$line" if ($addtodict > 5);
    }
    close(INPUT);
    close(OUTPUT);

    unlink($file);
}
#----------------------------------------------------------------------------------#


#----------------------------------------------------------------------------------#
# Parse StackOverflow Extensions                                                   #
#----------------------------------------------------------------------------------#
{
    my $file;
    {
        local $/;
        open(INPUTS, $inputs);
        $file = <INPUTS>;
        close(INPUTS);
    }

    my @inputs = split(",\n", $file);

    foreach my $line (@inputs)
    {
        my ($extension, $type);
        if ($line =~ /"(.+)",\s?"(.+)"/)
        {
            $extension = uc($1);
            $type = $2;

            next unless ($extension && $type);

            if (exists $extensions{$extension})
            {
                if ($extensions{$extension} !~ /$type/i)
                {
                    $conflicts++;
                    # $type = resolve($extension, $type, $extensions{$extension});
                }
                else
                {
                    $identical++;
                }
            }

            $extensions{$extension} = $type;
        }
    }

    $subtotals = (scalar @inputs);
    unlink($inputs);
}
#----------------------------------------------------------------------------------#


#----------------------------------------------------------------------------------#
# Display Statistics                                                               #
#----------------------------------------------------------------------------------#
{
    $grandtotal = (scalar (keys %extensions));

    print "\n\n";
    print "Sub Total A : $subtotala\n";
    print "Sub Total S : $subtotals\n";
    print "Identical   : $identical\n";
    print "Conflicts   : $conflicts\n";
    print "-------------------\n";
    print "Grand Total : $grandtotal\n\n";
}
#----------------------------------------------------------------------------------#


#----------------------------------------------------------------------------------#
# Publish Output                                                                   #
#----------------------------------------------------------------------------------#
{
    my @output;

    push (@output, "        [ContentTypeMetadata(Value=\"text/plain\", IsText = true)]\n        Text");
    push (@output, "        [ContentTypeMetadata(Value=\"application/octet-stream\", IsBinary = true)]\n        Binary");

    $extensions{'FormUrlEncoded'} = "application/x-www-form-urlencoded";

    foreach my $extension (sort keys %extensions)
    {
        next if ($extension =~ /^\d/); # Enum name cannot begin with a number
        next if ($extension =~ /[^a-zA-Z0-9]/); # Enum name can only contain alphanumeric values

        my $type = $extensions{$extension};
        my $flag = (($type =~ /^text/i) || ($type =~ /xml$/i) || ($type =~ /(json|javascript)/) || ($extension eq 'FormUrlEncoded')) ? "IsText" : "IsBinary";

        push (@output, "        [ContentTypeMetadata(Value=\"$type\", $flag = true)]\n        $extension");
    }

    open(OUTPUT, ">$output");
    print OUTPUT "    public enum ContentType\n    {\n";
    print OUTPUT join(",\n\n", @output);
    print OUTPUT "\n    }\n";
    close(OUTPUT);
}
#----------------------------------------------------------------------------------#