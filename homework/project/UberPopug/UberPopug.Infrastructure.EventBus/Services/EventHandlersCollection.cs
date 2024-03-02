using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Infrastructure.EventBus.Services;

 internal class EventHandlersCollection
   {
      private readonly Dictionary<string, List<Type>> handlers;
      private readonly HashSet<Type> eventTypes;

      internal EventHandlersCollection()
      {
         handlers = new Dictionary<string, List<Type>>();
         eventTypes = new HashSet<Type>();
      }

      public void AddHandler<TEventType, THandler>()
         where TEventType: IEventModel
         where THandler: IEventHandler<TEventType>
      {
         var eventKey = typeof(TEventType).Name;

         var handlerType = typeof(THandler);

         if (!handlers.ContainsKey(eventKey))
         {
            handlers[eventKey] = new List<Type>() { handlerType };
         }
         else
         {
            if (handlers[eventKey].Any(h => h == handlerType))
            {
               throw new ArgumentException($"Handler of type {handlerType.Name} already exists");
            }

            handlers[eventKey].Add(handlerType);
         }

         var eventType = typeof(TEventType);

         if (!eventTypes.Contains(eventType))
         {
            eventTypes.Add(eventType);
         }
      }

      public void RemoveHandler<TEventType, THandler>()
         where TEventType : IEventModel
         where THandler : IEventHandler<TEventType>
      {
         var eventKey = typeof(TEventType).Name;
         if (handlers.ContainsKey(eventKey))
         {
            if (handlers[eventKey].Contains(typeof(THandler)))
            {
               handlers[eventKey].Remove(typeof(THandler));

               if (handlers[eventKey].Count == 0) eventTypes.Remove(typeof(TEventType));
            }
         }
      }

      public List<Type> FindHandlersForEvent(string eventKey)
      {
         if (handlers.ContainsKey(eventKey))
            return handlers[eventKey];
         else return new List<Type>();
      }

      public bool HasSubscriptionForEvent(string eventKey)
      {
         return handlers.ContainsKey(eventKey);
      }

      public Type GetEventTypeByKey(string eventKey)
      {
         return eventTypes.First(e => e.Name == eventKey);
      }
   }
