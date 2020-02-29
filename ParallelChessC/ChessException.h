#pragma once

#include <exception>
#include <string>
class ChessException : public std::exception
{
public: 
    std::string message;
    ChessException(std::string message) {
        this->message = message;
    }

    virtual const char* what() const throw()
    {
        return message.c_str();
    }
};