using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Correspondence;
using Correspondence.Mementos;
using Correspondence.Strategy;
using System;
using System.IO;
using SwitchDemo;
/**
/ For use with http://graphviz.org/
digraph "SwitchDemo"
{
    rankdir=BT
    VideoSwitch -> Company [color="red"]
    VideoSwitch__selection -> VideoSwitch
    VideoSwitch__selection -> VideoSwitch__selection [label="  *"]
    VideoSwitch__selection -> RequestRoute
    RequestRoute -> VideoSwitch [color="red"]
    RequestRoute -> RequestRoute [label="  *"]
}
**/

namespace SwitchDemo
{
    public partial class Company : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Company newFact = new Company(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._identifier = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Company fact = (Company)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._identifier);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Company.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Company.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"SwitchDemo.Company", 8);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Company GetUnloadedInstance()
        {
            return new Company((FactMemento)null) { IsLoaded = false };
        }

        public static Company GetNullInstance()
        {
            return new Company((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Company> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Company)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles

        // Queries
        private static Query _cacheQueryVideoSwitches;

        public static Query GetQueryVideoSwitches()
		{
            if (_cacheQueryVideoSwitches == null)
            {
			    _cacheQueryVideoSwitches = new Query()
		    		.JoinSuccessors(VideoSwitch.GetRoleCompany())
                ;
            }
            return _cacheQueryVideoSwitches;
		}

        // Predicates

        // Predecessors

        // Fields
        private string _identifier;

        // Results
        private Result<VideoSwitch> _videoSwitches;

        // Business constructor
        public Company(
            string identifier
            )
        {
            InitializeResults();
            _identifier = identifier;
        }

        // Hydration constructor
        private Company(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _videoSwitches = new Result<VideoSwitch>(this, GetQueryVideoSwitches(), VideoSwitch.GetUnloadedInstance, VideoSwitch.GetNullInstance);
        }

        // Predecessor access

        // Field access
        public string Identifier
        {
            get { return _identifier; }
        }

        // Query result access
        public Result<VideoSwitch> VideoSwitches
        {
            get { return _videoSwitches; }
        }

        // Mutable property access

    }
    
    public partial class VideoSwitch : CorrespondenceFact
    {
        // Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				VideoSwitch newFact = new VideoSwitch(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				VideoSwitch fact = (VideoSwitch)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return VideoSwitch.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return VideoSwitch.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"SwitchDemo.VideoSwitch", 2045304702);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static VideoSwitch GetUnloadedInstance()
        {
            return new VideoSwitch((FactMemento)null) { IsLoaded = false };
        }

        public static VideoSwitch GetNullInstance()
        {
            return new VideoSwitch((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<VideoSwitch> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (VideoSwitch)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleCompany;
        public static Role GetRoleCompany()
        {
            if (_cacheRoleCompany == null)
            {
                _cacheRoleCompany = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "company",
			        Company._correspondenceFactType,
			        true));
            }
            return _cacheRoleCompany;
        }

        // Queries
        private static Query _cacheQuerySelection;

        public static Query GetQuerySelection()
		{
            if (_cacheQuerySelection == null)
            {
			    _cacheQuerySelection = new Query()
    				.JoinSuccessors(VideoSwitch__selection.GetRoleVideoSwitch(), Condition.WhereIsEmpty(VideoSwitch__selection.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQuerySelection;
		}
        private static Query _cacheQueryRequests;

        public static Query GetQueryRequests()
		{
            if (_cacheQueryRequests == null)
            {
			    _cacheQueryRequests = new Query()
    				.JoinSuccessors(RequestRoute.GetRoleVideoSwitch(), Condition.WhereIsEmpty(RequestRoute.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryRequests;
		}

        // Predicates

        // Predecessors
        private PredecessorObj<Company> _company;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<VideoSwitch__selection> _selection;
        private Result<RequestRoute> _requests;

        // Business constructor
        public VideoSwitch(
            Company company
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _company = new PredecessorObj<Company>(this, GetRoleCompany(), company);
        }

        // Hydration constructor
        private VideoSwitch(FactMemento memento)
        {
            InitializeResults();
            _company = new PredecessorObj<Company>(this, GetRoleCompany(), memento, Company.GetUnloadedInstance, Company.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
            _selection = new Result<VideoSwitch__selection>(this, GetQuerySelection(), VideoSwitch__selection.GetUnloadedInstance, VideoSwitch__selection.GetNullInstance);
            _requests = new Result<RequestRoute>(this, GetQueryRequests(), RequestRoute.GetUnloadedInstance, RequestRoute.GetNullInstance);
        }

        // Predecessor access
        public Company Company
        {
            get { return IsNull ? Company.GetNullInstance() : _company.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access
        public Result<RequestRoute> Requests
        {
            get { return _requests; }
        }

        // Mutable property access

        public TransientDisputable<VideoSwitch__selection, RequestRoute> Selection
        {
            get { return _selection.AsTransientDisputable(fact => (RequestRoute)fact.Value); }
			set
			{
				Community.Perform(async delegate()
				{
					var current = (await _selection.EnsureAsync()).ToList();
					if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
					{
						await Community.AddFactAsync(new VideoSwitch__selection(this, _selection, value.Value));
					}
				});
			}
        }
    }
    
    public partial class VideoSwitch__selection : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				VideoSwitch__selection newFact = new VideoSwitch__selection(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				VideoSwitch__selection fact = (VideoSwitch__selection)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return VideoSwitch__selection.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return VideoSwitch__selection.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"SwitchDemo.VideoSwitch__selection", -766482536);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static VideoSwitch__selection GetUnloadedInstance()
        {
            return new VideoSwitch__selection((FactMemento)null) { IsLoaded = false };
        }

        public static VideoSwitch__selection GetNullInstance()
        {
            return new VideoSwitch__selection((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<VideoSwitch__selection> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (VideoSwitch__selection)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleVideoSwitch;
        public static Role GetRoleVideoSwitch()
        {
            if (_cacheRoleVideoSwitch == null)
            {
                _cacheRoleVideoSwitch = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "videoSwitch",
			        VideoSwitch._correspondenceFactType,
			        false));
            }
            return _cacheRoleVideoSwitch;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        VideoSwitch__selection._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }
        private static Role _cacheRoleValue;
        public static Role GetRoleValue()
        {
            if (_cacheRoleValue == null)
            {
                _cacheRoleValue = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "value",
			        RequestRoute._correspondenceFactType,
			        false));
            }
            return _cacheRoleValue;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(VideoSwitch__selection.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<VideoSwitch> _videoSwitch;
        private PredecessorList<VideoSwitch__selection> _prior;
        private PredecessorObj<RequestRoute> _value;

        // Fields

        // Results

        // Business constructor
        public VideoSwitch__selection(
            VideoSwitch videoSwitch
            ,IEnumerable<VideoSwitch__selection> prior
            ,RequestRoute value
            )
        {
            InitializeResults();
            _videoSwitch = new PredecessorObj<VideoSwitch>(this, GetRoleVideoSwitch(), videoSwitch);
            _prior = new PredecessorList<VideoSwitch__selection>(this, GetRolePrior(), prior);
            _value = new PredecessorObj<RequestRoute>(this, GetRoleValue(), value);
        }

        // Hydration constructor
        private VideoSwitch__selection(FactMemento memento)
        {
            InitializeResults();
            _videoSwitch = new PredecessorObj<VideoSwitch>(this, GetRoleVideoSwitch(), memento, VideoSwitch.GetUnloadedInstance, VideoSwitch.GetNullInstance);
            _prior = new PredecessorList<VideoSwitch__selection>(this, GetRolePrior(), memento, VideoSwitch__selection.GetUnloadedInstance, VideoSwitch__selection.GetNullInstance);
            _value = new PredecessorObj<RequestRoute>(this, GetRoleValue(), memento, RequestRoute.GetUnloadedInstance, RequestRoute.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public VideoSwitch VideoSwitch
        {
            get { return IsNull ? VideoSwitch.GetNullInstance() : _videoSwitch.Fact; }
        }
        public PredecessorList<VideoSwitch__selection> Prior
        {
            get { return _prior; }
        }
        public RequestRoute Value
        {
            get { return IsNull ? RequestRoute.GetNullInstance() : _value.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class RequestRoute : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				RequestRoute newFact = new RequestRoute(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._fromChannel = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
						newFact._toChannel = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				RequestRoute fact = (RequestRoute)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._fromChannel);
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._toChannel);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return RequestRoute.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return RequestRoute.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"SwitchDemo.RequestRoute", 799546548);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static RequestRoute GetUnloadedInstance()
        {
            return new RequestRoute((FactMemento)null) { IsLoaded = false };
        }

        public static RequestRoute GetNullInstance()
        {
            return new RequestRoute((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<RequestRoute> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (RequestRoute)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleVideoSwitch;
        public static Role GetRoleVideoSwitch()
        {
            if (_cacheRoleVideoSwitch == null)
            {
                _cacheRoleVideoSwitch = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "videoSwitch",
			        VideoSwitch._correspondenceFactType,
			        true));
            }
            return _cacheRoleVideoSwitch;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        RequestRoute._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(RequestRoute.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}
        private static Query _cacheQueryFollowing;

        public static Query GetQueryFollowing()
		{
            if (_cacheQueryFollowing == null)
            {
			    _cacheQueryFollowing = new Query()
    				.JoinSuccessors(RequestRoute.GetRolePrior(), Condition.WhereIsEmpty(RequestRoute.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryFollowing;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<VideoSwitch> _videoSwitch;
        private PredecessorList<RequestRoute> _prior;

        // Fields
        private int _fromChannel;
        private int _toChannel;

        // Results
        private Result<RequestRoute> _following;

        // Business constructor
        public RequestRoute(
            VideoSwitch videoSwitch
            ,IEnumerable<RequestRoute> prior
            ,int fromChannel
            ,int toChannel
            )
        {
            InitializeResults();
            _videoSwitch = new PredecessorObj<VideoSwitch>(this, GetRoleVideoSwitch(), videoSwitch);
            _prior = new PredecessorList<RequestRoute>(this, GetRolePrior(), prior);
            _fromChannel = fromChannel;
            _toChannel = toChannel;
        }

        // Hydration constructor
        private RequestRoute(FactMemento memento)
        {
            InitializeResults();
            _videoSwitch = new PredecessorObj<VideoSwitch>(this, GetRoleVideoSwitch(), memento, VideoSwitch.GetUnloadedInstance, VideoSwitch.GetNullInstance);
            _prior = new PredecessorList<RequestRoute>(this, GetRolePrior(), memento, RequestRoute.GetUnloadedInstance, RequestRoute.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
            _following = new Result<RequestRoute>(this, GetQueryFollowing(), RequestRoute.GetUnloadedInstance, RequestRoute.GetNullInstance);
        }

        // Predecessor access
        public VideoSwitch VideoSwitch
        {
            get { return IsNull ? VideoSwitch.GetNullInstance() : _videoSwitch.Fact; }
        }
        public PredecessorList<RequestRoute> Prior
        {
            get { return _prior; }
        }

        // Field access
        public int FromChannel
        {
            get { return _fromChannel; }
        }
        public int ToChannel
        {
            get { return _toChannel; }
        }

        // Query result access
        public Result<RequestRoute> Following
        {
            get { return _following; }
        }

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Company._correspondenceFactType,
				new Company.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Company._correspondenceFactType }));
			community.AddQuery(
				Company._correspondenceFactType,
				Company.GetQueryVideoSwitches().QueryDefinition);
			community.AddType(
				VideoSwitch._correspondenceFactType,
				new VideoSwitch.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { VideoSwitch._correspondenceFactType }));
			community.AddQuery(
				VideoSwitch._correspondenceFactType,
				VideoSwitch.GetQuerySelection().QueryDefinition);
			community.AddQuery(
				VideoSwitch._correspondenceFactType,
				VideoSwitch.GetQueryRequests().QueryDefinition);
			community.AddType(
				VideoSwitch__selection._correspondenceFactType,
				new VideoSwitch__selection.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { VideoSwitch__selection._correspondenceFactType }));
			community.AddQuery(
				VideoSwitch__selection._correspondenceFactType,
				VideoSwitch__selection.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				RequestRoute._correspondenceFactType,
				new RequestRoute.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { RequestRoute._correspondenceFactType }));
			community.AddQuery(
				RequestRoute._correspondenceFactType,
				RequestRoute.GetQueryIsCurrent().QueryDefinition);
			community.AddQuery(
				RequestRoute._correspondenceFactType,
				RequestRoute.GetQueryFollowing().QueryDefinition);
		}
	}
}
