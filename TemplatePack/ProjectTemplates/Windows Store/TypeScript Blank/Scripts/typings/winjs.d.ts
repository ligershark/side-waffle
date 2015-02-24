/* *****************************************************************************
Copyright (c) Microsoft Corporation. All rights reserved. 
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0  
 
THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
MERCHANTABLITY OR NON-INFRINGEMENT. 
 
See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */

/**
  * The WinJS namespace provides special Windows Library for JavaScript functionality, 
  * including Promise and xhr.
  */
declare module WinJS {

    /**
      * An error object.
      */
    export class ErrorFromName {
        /**
          * Creates an Error object with the specified name and message properties.
          * @param name The name of this error. The name is meant to be consumed programmatically and should not be localized.
          * @param message The message for this error. The message is meant to be consumed by humans and should be localized.
          */
        constructor(name: string, message: string);
    }

    /**
      * This method has been deprecated. Strict processing is always on; you don't have to call this 
      * method to turn it on. 
      */
    export function strictProcessing(): void;

    /**
      * Provides functionality for data and template binding.
      */
    export module Binding {
        /**
          * Creates a default binding initializer for binding between a source property 
          * and a destination property with the specified converter function that is 
          * executed on the value of the source property.
          * @param convert {function} The conversion function that takes the source property and 
          * produces a value that is set to the destination property. This function must 
          * be accessible from the global namespace.
          */
        export function converter(convert: (type: any) => any): any;

        /**
          * Returns an observable object. This may be an observable proxy for the specified 
          * object, an existing proxy, or the specified object itself if it directly 
          * supports observation.
          * @param data The object to observe.
          */
        export function as(data: any): any;
        /**
          * Represents a list of objects that can be accessed by index or by a string key. 
          * Provides methods to search, sort, filter, and manipulate the data. 
          */
        export class List {
            /**
             * Gets a List that is a projection of the groups that were identified in 
             * this list. This property is available only for GroupsListProjection objects.
             */
            public groups: any;
            /**
              * Gets the IListDataSource for the list. The only purpose of this property is 
              * to adapt a List to the data model that is used by ListView and FlipView.
              */
            public dataSource: any;
            /**
              * Gets or sets the length of the list, which is an integer value one higher 
              * than the highest element defined in the list.
              */
            public length: number;
            /**
              * Creates a List object.
              * @param list The array containing the elements to initalize the list.
              * @param options You can set two Boolean options: binding and proxy. 
              */
            constructor(list: any[], options?: ListOptions);
            /**
              * Links the specified action to the property specified in the name parameter. This 
              * function is invoked when the value of the property may have changed. It is not 
              * guaranteed that the action will be called only when a value has actually changed, 
              * nor is it guaranteed that the action will be called for every value change. The 
              * implementation of this function coalesces change notifications, such that multiple 
              * updates to a property value may result in only a single call to the specified action.
              * @param name {string} The name of the property to which to bind the action.
              * @param action {function} The function to invoke asynchronously when the property may have changed.
              */
            public bind(name: string, action: () => void): any;
            /**
              * Returns a new list consisting of a combination of two arrays.
              * @param item Additional items to add to the end of the list.
              */
            public concat(...item): any[];
            /**
              * Checks whether the specified callback function returns true for all elements in a list.
              * @param callback {function} A function that accepts up to three arguments. This function is called for each element in the list until it returns false or the end of the list is reached.
              * @param thisArg An object to which the this keyword can refer in the callback function. If thisArg is omitted, undefined is used.
              */
            public every(callback: (a?: any, b?: any, c?: any) => boolean, thisArg: any): boolean;
            /**
              * Returns the elements of a list that meet the condition specified in a callback function.
              * @param callback {function} A function that accepts up to three arguments. The function is called for each element in the list. This function must always return the same results, given the same inputs. The results should not depend on values that are subject to change. You must call notifyMutated each time an item changes. Do not batch change notifications.
              * @param thisArg An object to which the this keyword can refer in the callback function. If thisArg is omitted, undefined is used.
              */
            public filter(callback: (a?: any, b?: any, c?: any) => boolean, thisArg: any): boolean;
            /**
              * Calls the specified callback function for each element in a list.
              * @param callback {function} A function that accepts up to three arguments. The function is called for each element in the list. The arguments are as follows: value: the value of the element. index: the current index. array: the array that contains the element.
              * @param thisArg An object to which the this keyword can refer in the callback function. If thisArg is omitted, undefined is used.
              */
            public forEach(callback: (item: any, index?: number, array?: any[]) => boolean, thisArg: any): void;
            /**
              * Creates a live filtered projection over this list. As the list changes, the 
              * filtered projection reacts to those changes and may also change.
              * @param A function that accepts a single argument. The createFiltered function calls the callback with each element in the list. If the function returns true, that element will be included in the filtered list. This function must always return the same results, given the same inputs. The results should not depend on values that are subject to change. You must call notifyMutated each time an item changes. Do not batch change notifications.
              */
            public createFiltered(predicate: (x: any) => boolean): List;
            /**
              * Creates a live grouped projection over this list. As the list changes, the 
              * grouped projection reacts to those changes and may also change. The grouped 
              * projection sorts all the elements of the list to be in group-contiguous order. 
              * The grouped projection also contains a .groups property, which is a List 
              * representing the groups that were found in the list.
              * @param groupKey {function} A function that accepts a single argument. The function is called with each element in the list, the function should return a string representing the group containing the element. This function must always return the same results, given the same inputs. The results should not depend on values that are subject to change. You must call notifyMutated each time an item changes. Do not batch change notifications.
              * @param groupData {function} A function that accepts a single argument. The function is called once, on one element per group. It should return the value that should be set as the data of the .groups list element for this group. The data value usually serves as summary or header information for the group.
              * @param groupSorter {function} A function that accepts two arguments. The function is called with pairs of group keys found in the list. It must return one of the following numeric values: negative if the first argument is less than the second (sorted before), zero if the two arguments are equivalent, positive if the first argument is greater than the second (sorted after).
              */
            public createGrouped(groupKey: (x: any) => any, groupData: (x: any) => any, groupSorter?: (x: any, y: any) => number): List;
            /**
              * Creates a live sorted projection over this list. As the list changes, the sorted projection reacts to those changes and may also change.
              * @param sorter {function} A function that accepts two arguments. The function is called with elements in the list. It must return one of the following numeric values: negative if the first argument is less than the second, zero if the two arguments are equivalent, positive if the first argument is greater than the second. This function must always return the same results, given the same inputs. The results should not depend on values that are subject to change. You must call notifyMutated each time an item changes. Do not batch change notifications.
              */
            public createdSorted(sorter: (x: any, y: any) => number): any;
            /**
              * Adds an event listener.
              * onitemchanged: An item in the list has changed its value.
              * oniteminserted: A new item has been inserted into the list.
              * onitemmutated: An item has been mutated. This event occurs as a result of calling the notifyMutated method.
              * onitemremoved: An item has been removed from the list.
              * onreload: The list has been refreshed. Any references to items in the list may be incorrect
              * @param eventName The event name.
              * @param eventCallback The event handler function to associate with this event.
              */
            public addEventListener(eventName: string, eventCallback: EventListener);
            /**
              * Raises an event of the specified type and with additional properties. 
              * @param The type (name) of the event.
              * @param The set of additional properties to be attached to the event object when the event is raised.
              */
            public dispatchEvent(type: string, eventProperties?: any): boolean;
            /**
              * Disconnects a WinJS.Binding.List projection from its underlying WinJS.Binding.List. It's 
              * only important to call this method when the WinJS.Binding.List projection and the 
              * WinJS.Binding.List have different lifetimes. (Call this method on the WinJS.Binding.List 
              * projection, not the underlying WinJS.Binding.List.) 
              */
            public dispose(): void;
            /**
              * Gets a key/data pair for the specified list index.
              * @param The index of value to retrieve.
              */
            public getItem(index: number): any;
            /**
              * Gets a key/data pair for the list item key specified.
              * @param The key of the value to retrieve.
              */
            public getItemFromKey(key: string): any;
            /**
            * Gets the index of the first occurrence of the specified value in a list.
            * @param searchElement The value to locate in the list.
            * @param startIndex The index at which to begin the search. If fromIndex is omitted, the search starts at index 0.
            */
            public indexOf(searchElement: any, startIndex?: number): number;
            /**
              * Gets the index of the first occurrence of a key in a list. 
              * @param The key to locate in the list.
              */
            public indexOfKey(key: string): number;
            /**
              * Returns a string consisting of all the elements of a list separated by the specified 
              * separator string.
              * @param separator A string used to separate the elements of a list. If this parameter is omitted, the list elements are separated with a comma.
              */
            public join(separator?: string): string;
            /**
              * Gets the index of the last occurrence of the specified value in a list.
              * @param searchElement The value to locate in the list.
              * @param fromIndex The index at which to begin the search. If fromIndex is omitted, the search starts at the last index in the list.
              */
            public lastIndexOf(searchElement: any, fromIndex?: number): number;
            /**
              * Calls the specified callback function on each element of a list, and returns an array 
              * that contains the results.
              * @param callback A function that accepts up to three arguments. The function is called for each element in the list.
              * @param thisArg An object to which the this keyword can refer in the callback function. If thisArg is omitted, undefined is used.
              */
            public map(callback: (a?: any, b?: any, c?: any) => boolean, thisArg: any): any[];
            /**
              * Moves the value at index to the specified position.
              * @param index The original index of the value.
              * @param newIndex The index of the value after the move.
              */
            public move(index: number, newIndex: number): void;
            /**
              * Notifies listeners that a property value was updated.
              * @param name The name of the property that is being updated.
              * @param newValue The new value for the property.
              * @param oldValue The old value for the property.
              */
            public notify(name, newValue, oldValue): Promise;
            /**
              * Forces the list to send a itemmutated notification to any listeners for the value at 
              * the specified index.
              * @param index The index of the value that was mutated.
              */
            public notifyMutated(index: number): void;
            /**
              * Forces the list to send a reload notification to any listeners.
              */
            public notifyReload(): void;
            /**
              * Removes the last element from a list and returns it.
              */
            public pop(): any;
            /**
              * Appends new element(s) to a list, and returns the new length of the list.
              * @param value The element to insert at the end of the list.
              */
            public push(value: any): number;
            /**
              * Accumulates a single result by calling the specified callback function for all 
              * elements in a list. The return value of the callback function is the accumulated 
              * result, and is provided as an argument in the next call to the callback function.
              * @param callback {function} A function that accepts up to four arguments. These arguments are: previousValue: The value from the previous call to the callback function. If an initialValue is provided to the reduce method, the previousValue is initialValue the first time the function is called. currentValue: the value of the current array element. currentIndex: The value of the current array element. array: The array object that contains the element.
              * @param initialValue If initialValue is specified, it is used as the value with which to start the accumulation. The first call to the function provides this value as an argument instead of a list value.
              */
            public reduce(callback: (previousValue?: any, currentValue?: any, currentIndex?: number, array?: any[]) => any, initialValue: any): any;
            /**
              * Accumulates a single result by calling the specified callback function for all 
              * elements in a list, starting with the last member of the list. The return value of 
              * the callback function is the accumulated result, and is provided as an argument in 
              * the next call to the callback function.
              * @param callback {function} A function that accepts up to four arguments. These arguments are: previousValue: The value from the previous call to the callback function. If an initialValue is provided to the reduce method, the previousValue is initialValue the first time the function is called. currentValue: the value of the current array element. currentIndex: The value of the current array element. array: The array object that contains the element.
              * @param initialValue If initialValue is specified, it is used as the value with which to start the accumulation. The first call to the function provides this value as an argument instead of a list value.
              */
            public reduceRight(callback: (previousValue?: any, currentValue?: any, currentIndex?: number, array?: any[]) => any, initialValue: any): any;
            /**
              * Removes an event listener.
              * @param eventName The event name.
              * @param eventCallback The event handler function to associate with this event.
              */
            public removeEventListener(eventName: string, eventCallback: EventListener): void;
            /**
              * Returns a list with the elements reversed. This method reverses the elements of a 
              * list object in place. It does not create a new list object during execution.
              */
            public reverse(): void;
            /**
              * Replaces the value at the specified index with a new value.
              * @param index The index of the value that was replaced.
              * @param newValue The new value.
              */
            public setAt(index: number, newValue: any): void;
            /**
              * Removes the first element from a list and returns it.
              */
            public shift(): any;
            /**
              * Extracts a section of a list and returns a new list.
              * @param begin The index that specifies the beginning of the section.
              * @param end The index that specifies the end of the section.
              */
            public slice(begin: number, end: number): List;

            /**
             * Checks whether the specified callback function returns true for any element of a list.
             * @param callback {function} A function that accepts up to three arguments. The function is called for each element in the list until it returns true, or until the end of the list.
             * @param thisArg An object to which the this keyword can refer in the callback function. If thisArg is omitted, undefined is used.
             */
            public some(callback: (a?: any, b?: any, c?: any) => boolean, thisArg: any): boolean;
            /**
              * Returns a list with the elements sorted. This method sorts the elements of a list 
              * object in place. It does not create a new list object during execution.
              * @param sortFunction The function used to determine the order of the elements. If omitted, the elements are sorted in ascending, ASCII character order. This function must always return the same results, given the same inputs. The results should not depend on values that are subject to change. You must call notifyMutated each time an item changes. Do not batch change notifications.
              */
            public sort(sortFunction?: (first: any, second: any) => number);
            /**
              * Removes elements from a list and, if necessary, inserts new elements in their place, returning the deleted elements.
              * @param start The zero-based location in the list from which to start removing elements.
              * @param howMany The number of elements to remove.
              * @param items The elements to insert into the list in place of the deleted elements.
              */
            public splice(start: number, howMany?: number, ...items: any[]): any[];
            /**
              * Removes one or more listeners from the notification list for a given property. 
              * @param name The name of the property to unbind. If this parameter is omitted, all listeners for all events are removed.
              * @param action The function to remove from the listener list for the specified property. If this parameter is omitted, all listeners are removed for the specific property.
              */
            public unbind(name: string, action?: any): any;
            /**
              * Appends new element(s) to a list, and returns the new length of the list.
              * @param value The element to insert at the start of the list.
              */
            public unshift(...value: any[]): number;
            /**
              * An item in the list has changed its value.
              */
            public onitemchanged: EventListener;
            /**
             * A new item has been inserted into the list.
             */
            public oniteminserted: EventListener;
            /**
             * An item is has been moved.
             */
            public onitemmoved: EventListener;
            /**
             * An item has been mutated. This event occurs as a result of calling the notifyMutated method.
             */
            public onitemmutated: EventListener;
            /**
             * An item has been removed from the list.
             */
            public onitemremoved: EventListener;
            /**
             * The list has been refreshed. Any references to items in the list may be incorrect
             */
            public onreload: EventListener;
        }
    }
    /**
      * Provides helper functions for defining namespaces.
      */
    export module Namespace {
        /**
          * Defines a new namespace with the specified name. 
          * @param name The name of the namespace. This could be a dot-separated name for nested namespaces.
          * @param members The members of the new namespace.
          */
        export function define(name: string, members: any): any;
        /**
          * Defines a new namespace with the specified name under the specified parent namespace.
          * @param parentNamespace The parent namespace.
          * @param name The name of the namespace. 
          * @param members The members of the new namespace.
          */
        export function defineWithParent(parentNamespace: string, name: string, members: any): any;
    }
    /**
      * Provides helper functions for defining Classes. 
      */
    export module Class {
        /**
          * Defines a class using the given constructor and the specified instance members.
          * @param constructor A constructor function that is used to instantiate this type.
          * @param instanceMembers The set of instance fields, properties, and methods made available on the type.
          * @param staticMembers The set of static fields, properties, and methods made available on the type.
          */
        export function define(constructor: Function, instanceMembers: any, staticMembers?: any): any;
        /**
          * Creates a sub-class based on the specified baseClass parameter, using prototype inheritance.
          * @param baseClass The type to inherit from.
          * @param constructor A constructor function that is used to instantiate this type.
          * @param instanceMembers The set of instance fields, properties, and methods made available on the type.
          * @param staticMembers The set of static fields, properties, and methods made available on the type.
          */
        export function derive(baseClass: any, constructor: Function, instanceMembers: any, staticMembers?: any): any;
        /**
          * Defines a class using the given constructor and the union of the set of instance members 
          * specified by all the mixin objects. The mixin parameter list is of variable length.
          * @param constructor A constructor function that will be used to instantiate this class.
          * @param mixin The mixin parameter list is of variable length.
          */
        export function mix(constructor: Function, mixin: any[]): any;
    }
    /**
      * Wraps calls to XMLHttpRequest in a promise.
      * You can use this function in Windows Store apps using JavaScript for cross-domain 
      * requests and intranet requests if you set the capabilities of your app to allow these operations
      * @param options The options that are applied to the XMLHttpRequest object.
      */
    export function xhr(options: XhrOptions): WinJS.Promise;
    /**
      * The options that are applied to the XMLHttpRequest object.
      */
    interface XhrOptions {
        /**
          * Optional. A string that specifies the HTTP method used to open the connection, such as GET, POST, or HEAD.
          * This parameter is not case-sensitive. If the type is not specified, the default is “GET”. For more details,
          * see the XMLHttpRequest.open documentation for the bstrMethod parameter.
          */
        type?: string;
        /**
          * Required. A string that specifies either the absolute or relative URL of the XML data or server-side 
          * XML Web services.
          */
        url: string;
        /**
          * Optional. A string that specifies the name of the user for authentication. If this parameter is empty 
          * or missing, and the site requires authentication, the component displays a logon window.
          */
        user?: string;
        /** 
          * Optional. A string that specifies the password for authentication. This parameter is ignored if the 
          * user parameter is empty or missing.
          */
        password?: string;
        /**
          * Optional. An object whose property names are used as header names and property values are used as 
          * header values, passed to the XMLHttpRequest.setRequestHeader method. See the XMLHttpRequest 
          * documentation for more details.
          */
        headers: any;
        /**
          * Required. An object containing data that is passed directly to the XMLHttpRequest.send method. 
          * See the XMLHttpRequest documentation for more details.
          */
        data: any;
        /**
          * Optional. A string that specifies the type of the expected response from a GET request.
          * one of: text, arraybuffer, blob, document, json, ms-stream
          */
        responseType: string;
    }

    /**
      * You can provide an implementation of this method yourself, or use WinJS.Utilities.startLog to 
      * create one that logs to the JavaScript console.
      * @param message The message to log.
      * @param tags The tag or tags to categorize the message (winjs, winjs controls, etc.).
      * @param type The type of message (error, warning, info, etc.).
      */
    export function log(message: string, tags: string, type: string): void;

    /**
      * Can be set to show the results of a validation process.
      */
    export var validation: boolean;
    /**
     *Provides application - level functionality, for example activation, storage, and application events.
   */
    export module Application {
        /**
          * Helper definition for reading and writing to various storage locations.
          */
        export interface IOHelper {
            /**
              * Determines whether the specified file exists in the folder.
              * @param filename The name of the file.
              */
            exists(filename: string): boolean;
            /**
              * Reads the specified file. If the file doesn't exist, the specified default value is returned.
              * @param filename The file to read from.
              * @param def The default value to be returned if the file failed to open.
              */
            readText(fileName: string, def?: string): WinJS.Promise;
            /**
              * Writes the specified text to the specified file.
              * @param filename The name of the file.
              * @param text The content to be written to the file.
              */
            writeText(fileName: string, text: string): WinJS.Promise;
            /**
              * Deletes a file from the folder.
              * @param filename The file to be deleted.
              */
            remove(fileName: string): WinJS.Promise;
        }
        /**
          * The local storage of the application
          */
        export var local: IOHelper;
        /**
          * The roaming storage of the application.
          */
        export var roaming: IOHelper;
        /**
          * The temp storage of the application.
          */
        export var temp: IOHelper;
        /**
          * An object used for storing app information that can be used to restore the app's state 
          * after it has been suspended and then resumed.
          * Data that can usefully be contained in this object includes the current navigation page 
          * or any information the user has added to the input controls on the page. You should not 
          * add information about customization (for example colors) or user-defined lists of content.
          */
        export var sessionState: any;
        export interface ApplicationActivationEvent extends Event {
            detail: any;
            setPromise(p: Promise): any;
        }
        /**
          * Adds an event listener for application-level events: activated, checkpoint, error, loaded, 
          * ready, settings, and unload.
          * Note:  Listeners for some Windows Store app events, like "suspending" and "resuming", cannot 
          * be added with this function. You must use Windows.UI.WebUI.WebUIApplication.addEventListener 
          * instead.
          * @param type The type (name) of the event. You can use any of the following:"activated", "checkpoint", "error", "loaded", "ready", "settings", and" unload".
          * @param listener The listener to invoke when the event is raised.
          * @param capture true to initiate capture, otherwise false.
          */
        export function addEventListener(type: string, listener: EventListener, capture?: boolean): void;
        /**
          * Queues a checkpoint event.
          */
        export function checkpoint(): void;
        /**
          * Queues an event to be processed by the WinJS.Application event queue.
          * The events that can be added to the WinJS.Application queue include the ones listed below. You 
          * can also add user-defined events to the queue.
          * Events: checkpoint, unload, activated, loaded,ready, settings, error,
          * @param eventRecord The event object is expected to have a type property that is used as the event name when dispatching on the WinJS.Application event queue. The entire object is provided to event listeners in the detail property of the event.
          */
        export function queueEvent(eventRecord: any): void;
        /**
          * Removes an event listener.
          * @param eventName The event name.
          * @param eventCallback The event handler function to associate with this event.
          * @param capture Specifies whether or not to initiate capture.
          */
        export function removeEventListener(eventName: string, eventCallback: EventListener, capture?: boolean): void;
        /**
          * Starts dispatching application events (the activated, checkpoint, error, loaded, ready, settings, and unload events). 
          */
        export function start(): void;
        /**
          * Stops application event processing and resets WinJS.Application to its initial state. 
          * All WinJS.Application event listeners (for the activated, checkpoint, error, loaded, ready, 
          * settings, and unload events) are removed.
          * Important: This function does not stop the execution of the app. 
          */
        export function stop(): void;
        /**
          * Occurs when WinRT activation has occurred. The name of this event is "activated" (and also 
          * "mainwindowactivated"). This event occurs after the loaded event and before the ready event.
          */
        export var onactivated: (evt: ApplicationActivationEvent) => void;
        /**
          * Occurs when receiving Process Lifetime Management (PLM) notification or when the checkpoint function is called.
          */
        export var oncheckpoint: EventListener;
        /**
          * Occurs when an unhandled error has been raised.
          */
        export var onerror: EventListener;
        /**
          * Occurs after the DOMContentLoaded event, which fires after the page has been parsed but 
          * before all the resources are loaded. This event occurs before the activated event and the 
          * ready event.
          */
        export var onloaded: EventListener;
        /**
          * Occurs when the application is ready. This event occurs after the loaded event and the 
          * activated event.
          */
        export var onready: EventListener;
        /**
          * Occurs when the settings charm is invoked.
          */
        export var onsettings: EventListener;
        /**
          * Occurs when the application is about to be unloaded.
          */
        export var onunload: EventListener;
    }

    /**
      * Provides a mechanism to schedule work to be done on a value that has not yet been 
      * computed. It is an abstraction for managing interactions with asynchronous APIs.
      */
    export class Promise {
        /**
          * A promise provides a mechanism to schedule work to be done on a value that has not yet 
          * been computed. It is a convenient abstraction for managing interactions with asynchronous APIs.
          * @param init {function} The function that is called during construction of the Promise that contains the implementation of the operation that the Promise will represent. This can be synchronous or asynchronous, depending on the nature of the operation. Note that placing code within this function does not automatically run it asynchronously; that must be done explicitly with other asynchronous APIs such as setImmediate, setTimeout, requestAnimationFrame, and the Windows Runtime asynchronous APIs. The init function is given three arguments: •completeDispatch: The init code should invoke this when the operation is complete, passing the operation's results as an argument. •errorDispatch: The init code should invoke this function when an error occurs, which will place the promise into the error state. The argument to errorDispatch should be an object created with WinJS.Promise.ErrorFromName. •progressDispatch: If the operation wishes to support progress, The init code should invoke this function periodically while the operation is underway, passing an intermediate result as the argument.
          * @param onCancel {function} The function to call if a consumer of this promise wants to cancel its undone work. Promises are not required to support cancellation.
          */
        constructor(init: (completeDispatch: any, errorDispatch: any, progressDispatch: any) => void, onCancel?: (e?: any) => any);
        /**
          * Adds an event listener for the promise.
          * @param type The type (name) of the event.
          * @param listener The listener to invoke when the event is raised.
          * @param capture true to initiate capture, otherwise false.
          */
        public addEventListener(type: string, listener: EventListener, capture?: boolean): void;
        /**
          * Returns a promise that is fulfilled when one of the input promises has been fulfilled.
          * @param value An array that contains Promise objects or objects whose property values include Promise objects.
          */
        public any(value: Promise[]): Promise;
        /**
          * Returns a promise. If the object is already a Promise it is returned; otherwise the object 
          * is wrapped in a Promise.
          * You can use this function when you need to treat a non-Promise object like a Promise, 
          * for example when you are calling a function that expects a promise, but already have the 
          * value needed rather than needing to get it asynchronously.
          * @param value The value to be treated as a Promise.
          */
        public as(value: any): Promise;
        /**
          * Attempts to cancel the fulfillment of a promised value. If the promise hasn't already 
          * been fulfilled and cancellation is supported, the promise enters the error state with 
          * a value of Error("Canceled").
          */
        public cancel(): void;
        /**
          * Allows you to specify the work to be done on the fulfillment of the promised value, 
          * the error handling to be performed if the promise fails to fulfill a value, and the 
          * handling of progress notifications along the way. After the handlers have finished 
          * executing, this function throws any error that would have been returned from then as 
          * a promise in the error state.
          * @param onComplete The function to be called if the promise is fulfilled successfully with a value. The fulfilled value is passed as the single argument. If the value is null, the fulfilled value is returned. The value returned from the function becomes the fulfilled value of the promise returned by then. If an exception is thrown while executing the function, the promise returned by then moves into the error state.
          * @param onError The function to be called if the promise is fulfilled with an error. The error is passed as the single argument. If it is null, the error is forwarded. The value returned from the function is the fulfilled value of the promise returned by then.
          * @param onProgress The function to be called if the promise reports progress. Data about the progress is passed as the single argument. Promises are not required to support progress.
          */
        public done(onComplete: (result: any) => void, onError?: (error: any) => void, onProgress?: (data: any) => void): void;
        /**
          * Allows you to specify the work to be done on the fulfillment of the promised value, 
          * the error handling to be performed if the promise fails to fulfill a value, and the handling 
          * of progress notifications along the way.
          * @param onComplete The function to be called if the promise is fulfilled successfully with a value. The value is passed as the single argument. If the value is null, the value is returned. The value returned from the function becomes the fulfilled value of the promise returned by then. If an exception is thrown while this function is being executed, the promise returned by then moves into the error state.
          * @param onError The function to be called if the promise is fulfilled with an error. The error is passed as the single argument. In different cases this object may be of different types, so it is necessary to test the object for the properties you expect. If the error is null, it is forwarded. The value returned from the function becomes the value of the promise returned by the then function.
          * @param onProgress The function to be called if the promise reports progress. Data about the progress is passed as the single argument. Promises are not required to support progress.
          */
        public then(onComplete: (result: any) => void, onError?: (error: any) => void, onProgress?: (data: any) => void): void;
        /**
          * Determines whether a value fulfills the promise contract.
          * @param value A value that may be a promise.
          */
        public is(value: any): boolean;
        /**
          * Creates a Promise that is fulfilled when all the values are fulfilled.
          * @param An object whose members contain values, some of which may be promises.
          */
        public join(value: any[]): Promise;
        /**
          * Performs an operation on all the input promises and returns a promise that has the 
          * shape of the input and contains the result of the operation that has been performed on each input.
          * @param values A set of values (which could be either an array or an object) of which some or all are promises.
          * @param complete The function to be called if the promise is fulfilled with a value. This function takes a single argument, which is the fulfilled value of the promise.
          * @param error The function to be called if the promise is fulfilled with an error. This function takes a single argument, which is the error value of the promise.
          * @param progress The function to be called if the promise reports progress. This function takes a single argument, which is the data about the progress of the promise. Promises are not required to support progress.
          */
        public thenEach(values: any, complete: (result: any) => void, error?: (error: any) => void, progress?: (data: any) => void): Promise;
        /**
          * This method has two forms: WinJS.Promise.timeout(timeout) and WinJS.Promise.timeout(timeout, promise).
          * WinJS.Promise.timeout(timeout) creates a promise that is completed asynchronously after 
          * the specified timeout, essentially wrapping a call to setTimeout within a promise. 
          * WinJS.Promise.timeout(timeout, promise) sets a timeout period for completion of the specified 
          * promise, automatically canceling the promise if it is not completed within the timeout period.
          * @param timeout The timeout period in milliseconds. If this value is zero or not specified, msSetImmediate is called, otherwise setTimeout is called.
          * @param promise Optional. A promise that will be canceled if it doesn't complete within the timeout period.
          */
        public timeout(timeout: number, promise?: Promise): Promise;
        /**
          * Removes an event listener.
          * @param eventName The event name.
          * @param eventCallback The event handler function to associate with this event.
          * @param capture Specifies whether or not to initiate capture.
          */
        public removeEventListener(eventName: string, eventCallback: EventListener, capture?: boolean): void;
        /**
          * Wraps a non-promise value in a promise. This method is like wrapError, which allows you 
          * to produce a Promise in error conditions, in that it allows you to return a Promise in success conditions.
          * @param value Some non-promise value to be wrapped in a promise. 
          */
        public wrap(value: any): Promise;
        /**
          * Wraps a non-promise error value in a promise. You can use this function if you need to 
          * pass an error to a function that requires a promise.
          * @param error A non-promise error value to be wrapped in a promise.
          */
        public wrapError(error: any): Promise;
        /**
          * Occurs when there is an error in processing a promise.
          * The onerror event occurs whenever a runtime error is caught in any promise, whether or 
          * not it is handled elsewhere. You can use the error handler to set breakpoints while you're 
          * debugging or to provide general error handling such as error logging. But because this is 
          * a general error handling mechanism, you might not get much detail about the exact code or 
          * user input that caused the error. 
          */
        public onerror: EventListener;
        // TODO: dispatchEvent --> http://msdn.microsoft.com/en-us/library/windows/apps/br211865.aspx
    }

    export module Navigation {
        export var history: any;
        export var canGoBack: boolean;
        export var canGoForward: boolean;
        export var location: string;
        export var state: any;
        export function addEventListener(type: string, listener: EventListener, capture: boolean): void;
        export function back(): void;
        export function forward(): void;
        export function navigate(location: any, initialState: any);
        export function navigate(location: any);
        export function removeEventListener(type: string, listener: EventListener, capture: boolean): void;
        export var onbeforenavigate: CustomEvent;
        export var onnavigated: CustomEvent;
        export var onnavigating: CustomEvent;
    }
    export module Utilities {
        export function markSupportedForProcessing(obj: any): void;
        export enum Key {
            backspace = 8,
            tab = 9,
            enter = 13,
            shift = 16,
            ctrl = 17,
            alt = 18,
            pause = 19,
            capsLock = 20,
            escape = 27,
            space = 32,
            pageUp = 33,
            pageDown = 34,
            end = 35,
            home = 36,
            leftArrow = 37,
            upArrow = 38,
            rightArrow = 39,
            downArrow = 40,
            insert = 45,
            deleteKey = 46,
            num0 = 48,
            num1 = 49,
            num2 = 50,
            num3 = 51,
            num4 = 52,
            num5 = 53,
            num6 = 54,
            num7 = 55,
            num8 = 56,
            num9 = 57,
            a = 65,
            b = 66,
            c = 67,
            d = 68,
            e = 69,
            f = 70,
            g = 71,
            h = 72,
            i = 73,
            j = 74,
            k = 75,
            l = 76,
            m = 77,
            n = 78,
            o = 79,
            p = 80,
            q = 81,
            r = 82,
            s = 83,
            t = 84,
            u = 85,
            v = 86,
            w = 87,
            x = 88,
            y = 89,
            z = 90,
            leftWindows = 91,
            rightWindows = 92,
            numPad0 = 96,
            numPad1 = 97,
            numPad2 = 98,
            numPad3 = 99,
            numPad4 = 100,
            numPad5 = 101,
            numPad6 = 102,
            numPad7 = 103,
            numPad8 = 104,
            numPad9 = 105,
            multiply = 106,
            add = 107,
            subtract = 109,
            decimalPoint = 110,
            divide = 111,
            f1 = 112,
            f2 = 113,
            f3 = 114,
            f4 = 115,
            f5 = 116,
            f6 = 117,
            f7 = 118,
            f8 = 119,
            f9 = 120,
            f10 = 121,
            f11 = 122,
            f12 = 123,
            numLock = 144,
            scrollLock = 145,
            semicolon = 186,
            equal = 187,
            comma = 188,
            dash = 189,
            period = 190,
            forwardSlash = 191,
            graveAccent = 192,
            openBracket = 219,
            backSlash = 220,
            closeBracket = 221,
            singleQuote = 222
        }
    }
    export module UI {

        export var process: any;
        export var processAll: any;
        export var ListLayout: any;
        export var GridLayout: any;
        export var Pages: any;
        export var Menu: any;
        export var setOptions: any;

        /**
          * Represents a selection of ListView items. 
          */
        interface Selection {
            /**
              * Adds one or more items to the selection.
              * @param The indexes or keys of the items to add. You can provide different types of objects for the items parameter: you can specify an index, a key, or a range of indexes. It can also be an array that contains one or more of these objects. For more info, see the Remarks section. 
              */
            add: (items: any) => WinJS.Promise;
            /**
              * Clears the selection.
              */
            clear: () => WinJS.Promise;
            /**
              * Returns the number of items in the selection.
              */
            count: () => number;
            /**
              * Returns an array that contains the items in the selection.
              */
            getItems: () => WinJS.Promise;
            /**
              * Returns a list of the indexes for the items in the selection.
              */
            getIndices: () => number[];
            /**
            * Gets an array of the index ranges for the selected items.
            */
            getRanges: () => any[];
            /**
              * Returns a value that indicates whether the selection contains every item in the data source.
              */
            isEverything: () => boolean;
            /**
              * Removes the specified items from the selection.
              * @param The indexes or keys of the items to remove. You can provide different types of objects for the items parameter: you can specify an index, a key, or a range of indexes. It can also be an array that contains one or more of these objects. 
              */
            remove: (items: any) => WinJS.Promise;
            /**
              * Clears the current selection and replaces it with the specified items. 
              * @param items The indexes or keys of the items that make up the selection. You can provide different types of objects for the items parameter: you can specify an index, a key, or a range of indexes. It can also be an array that contains one or more of these objects. 
              */
            set: (items: any) => WinJS.Promise;
            /**
              * Adds all the items in the ListView to the selection. 
              */
            selectAll: () => void;
        }
        class ListView {
            itemDataSource: any;
            /**
              * Registers an event handler for the specified event. 
              * @param type The name of the event to handle. See the ListView object page for a list of events. Note that you drop the "on" when specifying the event name for the addEventListener method. For example, instead of specifying "onselectionchange", you specify "selectionchange".
              * @param listener The event handler function to associate with the event.
              * @param capture Set to true to register the event handler for the capturing phase; otherwise, set to false to register the event handler for the bubbling phase. 
              */
            public addEventListener(type: string, listener: EventListener, capture?: boolean): void;
            /**
              *Gets or sets how the ListView reacts when the user taps or clicks an item. 
              */
            public tapBehavior: TapBehavior;
            public selection: Selection;
            public oncontentanimating: EventListener;
            public ongroupheaderinvoked: EventListener;
            public onitemdragstart: EventListener;
            public onitemdragenter: EventListener;
            public onitemdragend: EventListener;
            public onitemdragbetween: EventListener;
            public onitemdragleave: EventListener;
            public onitemdragchanged: EventListener;
            public onitemdragdrop: EventListener;
            public oniteminvoked: EventListener;
            public onkeyboardnavigating: EventListener;
            public onloadingstatechanged: EventListener;
            public onselectionchanging: EventListener;
            public onselectionchanged: EventListener;
        }
        /**
          * Specifies how items in a ListView respond to the tap interaction.
          */
        enum TapBehavior {
            /**
              * The item is selected and invoked. 
              */
            directSelect,
            /**
              * The item is selected if was not already selected, and its deselected if it was already selected. 
              */
            toggleSelect,
            /**
              * The item is invoked but not selected. 
              */
            invokeOnly,
            /**
              * Nothing happens. 
              */
            none
        }
        /**
          * Represents an application toolbar for displaying commands.
          */
        class AppBar {
            /**
              * Creates a new AppBar object.
              * @param element The DOM element that will host the control.
              * @param options The set of properties and values to apply to the new AppBar. 
              */
            constructor(element: HTMLElement, options?: any);
            /**
              * Sets the AppBarCommand objects that appear in the app bar.
              */
            public commands: any;
            /**
              * Gets or sets a value that indicates whether the AppBar is disabled.
              * Disabled app bars are hidden, do not react to edge events if not sticky, and may not be 
              * shown. If you set this property to true, then this app bar is disabled, and hide is called 
              * if the app bar is visible. If you set this property to false, then the app bar is enabled 
              * and will respond to edge events.
              */
            public disabled: boolean;
            /**
              * Gets the DOM element that hosts the AppBar.
              */
            public element: HTMLElement;
            /**
              * Gets a value that indicates whether the AppBar is hidden or in the process of becoming hidden.
              */
            public hidden: boolean;
            /**
              * Gets or sets the layout of the app bar contents.
              */
            public layout: string;
            /**
              * Gets or sets a value that specifies whether the AppBar appears at the 'top' or 'bottom' 
              * of the main view.
              */
            public placement: string;
            /**
              * Gets or sets a value that indicates whether the AppBar is sticky (won't light dismiss). If not 
              * sticky, the app bar dismisses normally when the user touches outside of the appbar.
              */
            public sticky: boolean;
            /**
              * Registers an event handler for the specified event. 
              * @param type The event type to register. It must be beforeshow, beforehide, aftershow, or afterhide.
              * @param listener The event handler function to associate with the event.
              * @param capture Set to true to register the event handler for the capturing phase; otherwise, set to false to register the event handler for the bubbling phase. 
              */
            public addEventListener(type: string, listener: EventListener, capture?: boolean): void;
            /**
              * Raises an event of the specified type and with additional properties. 
              * @param type The type (name) of the event.
              * @param eventProperties The set of additional properties to be attached to the event object when the event is raised.
              */
            public dispatchEvent(type: string, eventProperties: any): boolean;
            /**
              * Releases resources held by this AppBar. Call this method when the AppBar is no longer 
              * needed. After calling this method, the AppBar becomes unusable. 
              */
            public dispose(): void;
            /**
              * Returns the AppBarCommand object identified by id.
              * @param id The element idenitifier (ID) of the command to be returned.
              */
            public getCommandById(id: string): AppBarCommand;
            /**
             * Hides the AppBar.
             */
            public hide(): void;
            /**
              * Hides the specified commands of the AppBar.
              * @param commands The commands to hide. The array elements may be AppBarCommand objects, or the string identifiers (IDs) of commands.
              */
            public hideCommands(commands: any[]): void;
            /**
              * Removes an event listener.
              * @param type The event type to unregister. It must be beforeshow, beforehide, aftershow, or afterhide.
              * @param listener The event handler function to remove.
              * @param capture Specifies whether or not to initiate capture.
              */
            public removeEventListener(type: string, listener: EventListener, capture?: boolean): void;

            /**
              * Shows the AppBar if it is not disabled. 
              */
            public show(): void;
            /**
              * Shows the specified commands of the AppBar.
              * @param commands The commands to show. The array elements may be AppBarCommand objects, or the string identifiers (IDs) of commands.              
              */
            public showCommands(commands: any[]): void;
            /**
              * Shows the specified commands of the AppBar while hiding all other commands.
              * @param commands The commands to show. The array elements may be AppBarCommand objects, or the string identifiers (IDs) of commands.
              */
            public showOnlyCommands(commands: any[]): void;
            /**
              * Occurs immediately after the AppBar is hidden.
              */
            public onafterhide: EventListener;
            /**
              * Occurs after the AppBar is shown.
              */
            public onaftershow: EventListener;
            /**
              * Occurs before the AppBar is hidden
              */
            public onbeforehide: EventListener;
            /**
              * Occurs before a hidden AppBar is shown.
              */
            public onbeforeshow: EventListener;
        }
        /**
          * Represents a command to be displayed in an app bar.
          */
        class AppBarCommand {
            /**
              * Creates a new AppBarCommand object.
              * @param element The DOM element that will host the control.
              * @param options The set of properties and values to apply to the new AppBarCommand. 
              */
            constructor(element: HTMLElement, options?: any);
            /**
              * Gets or sets a value that indicates whether the AppBarCommand is disabled.
              */
            public disabled: boolean;
            /**
              * Gets the DOM element that hosts the AppBarCommand.
              */
            public element: HTMLElement;
            /**
              * Adds an extra CSS class during construction.
              */
            public extraClass: string;
            /**
              * Gets or sets the HTMLElement with a 'content' type AppBarCommand that should receive focus 
              * whenever focus moves by the user pressing HOME or the arrow keys, from the previous 
              * AppBarCommand to this AppBarCommand. Returns the AppBarCommand object's host element by default.
              */
            public firstElementFocus: HTMLElement;
            /**
              * Gets or sets the Flyout object displayed by this command. The specified flyout is shown when 
              * the AppBarCommand's button is invoked.
              */
            public flyout: Flyout;
            /**
              * Gets or sets a value that indicates whether the AppBarCommand is hiding or in the process of becoming hidden.
              */
            public hidden: boolean;
            /**
              * Gets or sets the icon of the AppBarCommand.
              */
            public icon: string;
            /**
              * Gets the element identifier (ID) of the command.
              */
            public id: string;
            /**
              * Gets or sets the label of the command.
              */
            public label: string;
            /**
              * Gets or sets the HTMLElement with a 'content' type AppBarCommand that should 
              * receive focus whenever focus moves by the user pressing END or the arrow keys, 
              * from the previous AppBarCommand to this AppBarCommand. Returns the AppBarCommand 
              * object's host element by default.
              */
            public lastElementFocus: HTMLElement;
            /**
              * Gets or sets the function to be invoked when the command is clicked.
              */
            public onclick: EventListener;
            /**
              * Gets or sets the function to be invoked when the command is clicked. 
              * The string "selection" if the command is in the selection section; otherwise "global", 
              * indicating that the command is in the global section.
              * "selection" is for commands that are meant to show in the context of selected objects. "global" 
              * is for commands that always show on a specific page.
              * "global" maps to right and "selection" maps to left in LTR (left-to-right) layouts. The opposite 
              * is true in RTL (right-to-left) layouts.
              */
            public section: string;
            /**
              * Gets or sets the selected state of a toggle button.
              * true if the toggle button is selected; otherwise, false. This is valid only when the type is "toggle".
              */
            public selected: boolean;
            /**
              * Gets or sets the tooltip of the command.
              */
            public tooltip: string;
            /**
              * Gets the type of the command.
              * The command can be one of the following types: "button","toggle","flyout","separator","content"
              */
            public type: string;
            /**
              * Adds an event listener.
              * @param type The type(name) of the event.
              * @param listener The listener to invoke when the event is raised.
              * @param capture true to initiate capture, otherwise false.
              */
            public addEventListener(type: string, listener: EventListener, capture?: boolean): void;
            /**
              * Releases resources held by this AppBarCommand. Call this method when the 
              * AppBarCommand is no longer needed. After calling this method, the AppBarCommand 
              * becomes unusable. 
              */
            public dispose(): void;
            /**
              * Removes an event listener.
              * @param eventName The event name.
              * @param eventCallback The event handler function to associate with this event.
              * @param capture Specifies whether or not to initiate capture.
              */
            public removeEventListener(eventName: string, eventCallback: EventListener, capture?: boolean): void;

        }

        class Flyout {
        }
    }
}

/**
  * Options for WinJS.Binding.List
  */
interface ListOptions {
    /**
      * If options.binding is true, the list contains the result of calling as on
      * the element values. 
      */
    binding?: boolean;
    /**
      * If options.proxy is true, the list specified as the first parameter is 
      * used as the storage for the List. This option should be used with care, 
      * because uncoordinated edits to the data storage may result in errors.
      */
    proxy?: boolean;
}

interface Element {
    winControl: any; // TODO: This should be control?   
}

