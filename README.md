# Medium

####**Your medium amongst webhooks.**

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=master)](https://travis-ci.org/noordwind/Medium)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Medium.svg?branch=develop)](https://travis-ci.org/noordwind/Medium/branches)


**What is Medium?**
----------------

It's a library built in order to help you consume the webhooks that can be invoked from different services like build servers, package managers, source control systems, custom APIs etc. 
and based on the received input validate such requests and execute any type of actions that you would like.

For example, you might be using a service A that is capable of sending webhooks to the given URL. On the other hand, there could be a service B, that you would like to 
send a custom request to, based on the input from service A. This is where the **Medium** comes in handy - it can act as as a sort of middleware, that will process the 
request from service A, validate it based on the set of defined rules, transform (if needed) into a separate object and eventually send a request to the service B (or more services).

With **Medium** you can build any sort of pipeline that you wish (e.g. sophisticated deployment strategy), based on the webhooks.

